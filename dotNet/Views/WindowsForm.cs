using Atomus.Control.Grid;
using Atomus.Diagnostics;
using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atomus.Control.Dictionary
{
    public partial class WindowsForm : Form, IDictionaryForm
    {
        IDictionary IDictionaryForm.Dictionary { get; set; }

        #region Init
        public WindowsForm()
        {
            InitializeComponent();
            
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Opacity = 0;//투명하게
            this.ControlBox = false;
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.Size = new Size(0, 0);
            this.KeyPreview = true;

            this.GridInit(this.DataGridView1);
        }

        private void GridInit(DataGridView dataGridView)
        {
            IDataGridAgent gridAgent;

            gridAgent = this.DataGridAgent(dataGridView);
            gridAgent.Init(EditAble.False, AddRows.False, DeleteRows.False, ResizeRows.False, AutoSizeColumns.False, AutoSizeRows.False, ColumnsHeadersVisible.True, EnableMenu.True, MultiSelect.False, Alignment.MiddleCenter, -1, 1, -1, RowHeadersVisible.False, Selection.FullRowSelect);
        }
        #endregion

        #region Dictionary
        #endregion

        #region Spread
        private void SetGrid(DataGridView dataGridView, DataView dataView, IDictionary dictionary)
        {
            IDataGridAgent gridAgent;
            Alignment textAlign;
            string tmp;
            string[] caption;
            string[] split;
            int width;
            int controlIndex;
            int index;
            int tmpIndex;
            ColumnVisible visible;

            try
            {
                caption = new string[1];

                //_GridAgent = (IDataGridAgent)Factory.CreateInstance("Atomus.Control.Grid.DataGridViewAgent");
                //_GridAgent.GridControl = dataGridView;
                gridAgent = this.DataGridAgent(dataGridView);
                gridAgent.Clear();

                gridAgent.Init(EditAble.False, AddRows.False, DeleteRows.False, ResizeRows.False, AutoSizeColumns.False, AutoSizeRows.False, ColumnsHeadersVisible.True, EnableMenu.True, MultiSelect.False, Alignment.MiddleCenter, -1, 1, -1, RowHeadersVisible.False, Selection.FullRowSelect);

                //컨트롤 인덱스 구하기
                controlIndex = 0;
                foreach (System.Windows.Forms.Control control in dictionary.Controls)
                {
                    if (control == null)
                    {
                        controlIndex += 1;
                        continue;
                    }

                    if (control.Equals(dictionary.CurrentControl))
                        break;

                    controlIndex += 1;
                }

                //현재 컨트롤의 컬럼이 앞으로 갈 수 있도록 컬럼을 먼저추가
                index = -1;

                foreach (DataColumn dataColumn in dataView.Table.Columns)
                {
                    if (dataColumn.DataType.IsNumeric())//숫자일 경우에 오른쪽 정렬
                        textAlign = Alignment.MiddleRight;
                    else
                        textAlign = Alignment.MiddleLeft;

                    tmp = dataColumn.Caption;

                    if (tmp.Contains("^"))
                        index += 1;

                    if (index == controlIndex)
                    {
                        split = tmp.Split('^');

                        caption[0] = split[0];
                        width = split[1].ToInt();

                        gridAgent.AddColumn(width, ColumnVisible.True, EditAble.False, Filter.False, Merge.False, Sort.NotSortable, null, textAlign, string.Empty, dataColumn.ColumnName, caption);
                        break;
                    }
                }

                tmpIndex = -1;
                foreach (DataColumn dataColumn in dataView.Table.Columns)
                {
                    tmpIndex += 1;

                    //앞에서 추가된 컬럼은 추가 하지 않음
                    if (tmpIndex == index)
                        continue;

                    if (dataColumn.DataType.IsNumeric())//숫자일 경우에 오른쪽 정렬
                        textAlign = Alignment.MiddleRight;
                    else
                        textAlign = Alignment.MiddleLeft;

                    tmp = dataColumn.Caption;

                    if (tmp.Contains("^"))
                    {
                        split = tmp.Split('^');

                        caption[0] = split[0];
                        width = split[1].ToInt();
                        visible = ColumnVisible.True;
                    }
                    else
                    {
                        caption[0] = tmp;
                        width = 0;
                        visible = ColumnVisible.False;
                    }

                    gridAgent.AddColumn(width, visible, EditAble.False, Filter.False, Merge.False, Sort.NotSortable, null, textAlign, string.Empty, dataColumn.ColumnName, caption);
                }

                //_GridAgent.RemoveColumnFiter(this.txt_Search);
                //_GridAgent.AddColumnFiter(0, this.txt_Search, this.ckb_SearchAll.Checked, false, AutoCompleteMode.SuggestAppend);

                dataGridView.DataSource = dataView;
            }
            catch (Exception exception)
            {
                DiagnosticsTool.MyTrace(exception);
            }
        }
        #endregion

        #region IO
        #endregion

        #region Event
        /// <summary>
        /// 폼이 포커스를 잃고 비활성 상태가 될 때 발생합니다.
        /// </summary>
        /// <param name="sender"></param>
        private void WindowsForm_Deactivate(object sender, EventArgs e)
        {
            IDictionaryForm form;

            try
            {
                form = (IDictionaryForm)sender;

                ThreadPool.QueueUserWorkItem(new WaitCallback(form.DeactivateCallback), sender);
            }
            catch (Exception exception)
            {
                DiagnosticsTool.MyTrace(exception);
            }
        }

        /// <summary>
        /// 폼의 리소스가 모두 해제 되는 것을 막고 폼을 숨깁니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form form;

            try
            {
                form = (Form)sender;

                if (form.Visible)
                {
                    e.Cancel = true;
                    this.WindowsForm_Deactivate(sender, null);
                }
            }
            catch (Exception exception)
            {
                DiagnosticsTool.MyTrace(exception);
            }
        }

        /// <summary>
        /// 폼의 크기가 변경되면 ListOfValueInfo에 크기를 전달합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowsForm_SizeChanged(object sender, EventArgs e)
        {
            if (((IDictionaryForm)this).Dictionary != null)
                ((IDictionaryForm)this).Dictionary.DictionaryFormSize = this.Size;
        }

        /// <summary>
        /// 폼이 나타나면 수행을 합니다.
        /// 1. BeforeAction 실행
        /// 2. 검색 문자열 가져오기
        /// 3. 전체 검색 여부 가져오기
        /// 4. 폼 크기 지정
        /// 5. 검색
        /// 6. 포커스 이동
        /// 7. 폼 위치 지정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowsForm_VisibleChanged(object sender, EventArgs e)
        {
            IBeforeEventArgs beforeEventArgs;

            try
            {
                if (this.Visible)
                {
                    this.TextBox_Search.Text = ((IDictionaryForm)this).Dictionary.CurrentControl.Text;

                    beforeEventArgs = ((IDictionaryForm)this).Dictionary;
                    this.CheckBox_SearchAll.Checked = beforeEventArgs.SearchAll;

                    this.Text_Search_KeyDown(null, new KeyEventArgs(Keys.Enter));
                }
            }
            catch (Exception exception)
            {
                DiagnosticsTool.MyTrace(exception);
            }
        }

        /// <summary>
        /// ESC 키를 누르면 LOV 폼을 닫는다
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowsForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)Enum.ToObject(typeof(Keys), Convert.ToInt32(e.KeyChar)) == Keys.Escape)
                this.WindowsForm_Deactivate(this, null);
        }

        /// <summary>
        /// 폼이 포커스를 잃고 비활성 상태가 될 때 Callback을 발생합니다.
        /// 폼을 닫고 리소스를 해제합니다.
        /// </summary>
        /// <param name="sender"></param>
        void IDictionaryForm.Deactivate(Form sender)
        {
            WaitCallback waitCallback;

            try
            {
                if (sender.InvokeRequired)
                {
                    waitCallback = new WaitCallback(((IDictionaryForm)this).DeactivateCallback);
                    this.Invoke(waitCallback, new object[] { (object)sender });
                }
                else
                {
                    sender.Visible = false;
                    sender.Opacity = 0;
                }
            }
            catch (Exception exception)
            {
                DiagnosticsTool.MyTrace(exception);
            }
        }
        
        /// <summary>
        /// 폼이 포커스를 잃고 비활성 상태가 될 때 Callback을 발생합니다.
        /// 폼을 닫고 리소스를 해제합니다.
        /// </summary>
        /// <param name="_Form"></param>
        void IDictionaryForm.DeactivateCallback(object sender)
        {
            WaitCallback waitCallback;
            Form form;

            try
            {
                form = (Form)sender;

                if (form.InvokeRequired)
                {
                    waitCallback = new WaitCallback(((IDictionaryForm)this).DeactivateCallback);
                    this.Invoke(waitCallback, sender);
                }
                else
                {
                    form.Visible = false;
                    form.Opacity = 0;
                }
            }
            catch (Exception exception)
            {
                DiagnosticsTool.MyTrace(exception);
            }
        }

        /// <summary>
        ///  리스트를 마우스로 더블클릭 했을때 grid_LOV_KeyDown 프로시져를 호출합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                this.DataGridView1_KeyDown(sender, new KeyEventArgs(Keys.Enter));
        }

        /// <summary>
        /// 그리드에서 키 입력에 따라 처리를 합니다.
        /// Keys.Up : 처음 행 일경우에 검색 컨트롤로 포커스를 이동합니다.
        /// Keys.Enter : 현재 행을 ListOfValueInfo.Result에 지정하고 Lov 컨트롤에 ContainerAction을 호출합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView dataGridView;
            DataRowView dataRowView;
            IAfterEventArgs afterEventArgs;
            IAction action;

            try
            {
                dataGridView = (DataGridView)sender;

                if (e.KeyCode == Keys.Up)
                    if (dataGridView.CurrentRow == null || dataGridView.CurrentRow.Index == 0)
                        this.TextBox_Search.Focus();

                if (e.KeyCode == Keys.Enter)
                {
                    afterEventArgs = ((IDictionaryForm)this).Dictionary;

                    if (dataGridView.SelectedRows.Count == 1)
                    {
                        dataRowView = (DataRowView)dataGridView.CurrentRow.DataBoundItem;//선택행 가져오기

                        if (dataRowView != null)
                            afterEventArgs.DataRow = dataRowView.Row;
                        else
                            afterEventArgs.DataRow = null;
                    }
                    else
                        afterEventArgs.DataRow = null;

                    action = (IAction)((IDictionaryForm)this).Dictionary;
                    action.ControlAction(((IDictionaryForm)this).Dictionary, "SetResult");

                    this.TextBox_Search.Focus();
                    this.WindowsForm_Deactivate(this, null);
                }
            }
            catch (Exception exception)
            {
                DiagnosticsTool.MyTrace(exception);
            }
        }

        /// <summary>
        /// 수정되는 검색 Text를 Dictionary 컨트롤에 반영합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Text_Search_TextChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.Control control;
            DataView dataView;
            StringBuilder stringBuilder;
            Decimal decimalTmp;
            IBeforeEventArgs beforeEventArgs;
            int controlIndex;
            int index;
            IDictionary dictionary;
            string text;

            try
            {
                control = (System.Windows.Forms.Control)sender;

                if (this.Visible)//Lov 폼이 활성화 상태에서
                {
                    if (control.Text != null)
                    {
                        text = control.Text;
                        text = text.Replace("[", "[[").Replace("]", "]]").Replace("[[", "[[]").Replace("]]", "[]]").Replace("*", "[*]").Replace("%", "[%]").Replace("'", "''");
                    }
                    else
                        text = "";

                    dictionary = ((IDictionaryForm)this).Dictionary;
                    if (dictionary.CurrentControl is TextBox)
                        dictionary.CurrentControl.Text = control.Text;//검색 문자열 반영

                    dataView = (DataView)this.DataGridView1.DataSource;

                    if (dataView == null)
                        return;

                    stringBuilder = new StringBuilder();

                    if (!text.Equals(""))//쿼리로 검색된 내용에서 RowFilter
                    {
                        beforeEventArgs = dictionary;

                        controlIndex = 0;
                        foreach (System.Windows.Forms.Control _Cont in dictionary.Controls)
                        {
                            if (_Cont == null)
                            {
                                controlIndex += 1;
                                continue;
                            }

                            if (_Cont.Equals(dictionary.CurrentControl))
                                break;

                            controlIndex += 1;
                        }

                        index = -1;
                        foreach (DataGridViewColumn _DataColumn in this.DataGridView1.Columns)
                        {
                            if (_DataColumn.Visible && _DataColumn.Width > 0) //보이는 컬럼만 검색
                            {
                                index += 1;

                                if (controlIndex == index && !this.CheckBox_SearchAll.Checked)
                                {
                                    if (_DataColumn.DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleRight)
                                        if (text.ToTryDecimal(out decimalTmp))
                                            if (beforeEventArgs.StartsWith)
                                                stringBuilder.AppendFormat("OR Convert([{0}], 'System.String') LIKE '{1}%' ", _DataColumn.DataPropertyName, text);
                                            //_Tmp += "OR Convert([" + _DataColumn.DataPropertyName + "], 'System.String') LIKE '" + _Text + "%' ";
                                            else
                                                stringBuilder.AppendFormat("OR Convert([{0}], 'System.String') LIKE '%{1}%' ", _DataColumn.DataPropertyName, text);
                                        //_Tmp += "OR Convert([" + _DataColumn.DataPropertyName + "], 'System.String') LIKE '%" + _Text + "%' ";
                                        else if (beforeEventArgs.StartsWith)
                                            stringBuilder.AppendFormat("OR [{0}] LIKE '{1}%' ", _DataColumn.DataPropertyName, text);
                                        //_Tmp += "OR [" + _DataColumn.DataPropertyName + "] LIKE '" + _Text + "%' ";
                                        else
                                            stringBuilder.AppendFormat("OR [{0}] LIKE '%{1}%' ", _DataColumn.DataPropertyName, text);
                                    //_Tmp += "OR [" + _DataColumn.DataPropertyName + "] LIKE '%" + _Text + "%' ";
                                    else if (beforeEventArgs.StartsWith)
                                        stringBuilder.AppendFormat("OR [{0}] LIKE '{1}%' ", _DataColumn.DataPropertyName, text);
                                    //_Tmp += "OR [" + _DataColumn.DataPropertyName + "] LIKE '" + _Text + "%' ";
                                    else
                                        stringBuilder.AppendFormat("OR [{0}] LIKE '%{1}%' ", _DataColumn.DataPropertyName, text);
                                    //_Tmp += "OR [" + _DataColumn.DataPropertyName + "] LIKE '%" + _Text + "%' ";

                                    break;
                                }

                                if (this.CheckBox_SearchAll.Checked)
                                    if (_DataColumn.DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleRight)
                                        if (text.ToTryDecimal(out decimalTmp))
                                            if (beforeEventArgs.StartsWith)
                                                stringBuilder.AppendFormat("OR Convert([{0}], 'System.String') LIKE '{1}%' ", _DataColumn.DataPropertyName, text);
                                            //_Tmp += "OR Convert([" + _DataColumn.DataPropertyName + "], 'System.String') LIKE '" + _Text + "%' ";
                                            else
                                                stringBuilder.AppendFormat("OR Convert([{0}], 'System.String') LIKE '%{1}%' ", _DataColumn.DataPropertyName, text);
                                        //_Tmp += "OR Convert([" + _DataColumn.DataPropertyName + "], 'System.String') LIKE '%" + _Text + "%' ";
                                        else
                                            if (beforeEventArgs.StartsWith)
                                            stringBuilder.AppendFormat("OR [{0}] LIKE '{1}%' ", _DataColumn.DataPropertyName, text);
                                        //_Tmp += "OR [" + _DataColumn.DataPropertyName + "] LIKE '" + _Text + "%' ";
                                        else
                                            stringBuilder.AppendFormat("OR [{0}] LIKE '%{1}%' ", _DataColumn.DataPropertyName, text);
                                    //_Tmp += "OR [" + _DataColumn.DataPropertyName + "] LIKE '%" + _Text + "%' ";
                                    else
                                        if (beforeEventArgs.StartsWith)
                                        stringBuilder.AppendFormat("OR [{0}] LIKE '{1}%' ", _DataColumn.DataPropertyName, text);
                                    //_Tmp += "OR [" + _DataColumn.DataPropertyName + "] LIKE '" + _Text + "%' ";
                                    else
                                        stringBuilder.AppendFormat("OR [{0}] LIKE '%{1}%' ", _DataColumn.DataPropertyName, text);
                                //_Tmp += "OR [" + _DataColumn.DataPropertyName + "] LIKE '%" + _Text + "%' ";
                            }
                        }
                    }

                    try
                    {
                        if (stringBuilder.ToString().StartsWith("OR "))
                            dataView.RowFilter = stringBuilder.ToString(3, stringBuilder.Length - 3);
                        else
                            dataView.RowFilter = stringBuilder.ToString();
                    }
                    catch (Exception exception)
                    {
                        dataView.RowFilter = "";
                        DiagnosticsTool.MyTrace(exception);
                    }

                    this.Text = string.Format("0} - {1} ({2}/{3})", dataView.Table.TableName, this.Name, dataView.Count, dataView.Table.Rows.Count);
                }
            }
            catch (Exception exception)
            {
                DiagnosticsTool.MyTrace(exception);
            }
        }

        /// <summary>
        /// 그리드로 포커스를 이동합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Text_Search_KeyDown(object sender, KeyEventArgs e)
        {
            IAfterEventArgs afterEventArgs;
            IAction commonAction;
            Task<bool> task;
            //bool _IsOK;

            if (e.KeyCode == Keys.Down)
                if (this.DataGridView1.RowCount > 0)
                    this.DataGridView1.Focus();

            if (e.KeyCode == Keys.Enter)
                try
                {
                    commonAction = (IAction)((IDictionaryForm)this).Dictionary;

                    task = (Task<bool>)commonAction.ControlAction(this, "SearchAsync");

                    if (await task)
                    {
                        afterEventArgs = ((IDictionaryForm)this).Dictionary;

                        if (afterEventArgs.DataTable == null)
                            return;

                        this.SetGrid(this.DataGridView1, afterEventArgs.DataTable.DefaultView, ((IDictionaryForm)this).Dictionary);
                        SetForm(this, this.DataGridView1);

                        if (afterEventArgs.DataTable != null)//결과가 있으면
                            this.Text = string.Format("{0} - {1} ({2}/{3})", afterEventArgs.DataTable.TableName, this.Name, afterEventArgs.DataTable.Rows.Count, afterEventArgs.DataTable.Rows.Count);
                        else//결과가 없으면
                            this.Text = string.Format("{0} - {1}", afterEventArgs.DataTable.TableName, this.Name);
                    }
                }
                catch (Exception exception)
                {
                    DiagnosticsTool.MyTrace(exception);
                }
        }
        
        ///// <summary>
        ///// 엔터키를 입력할 경우 쿼리합니다.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //}

        /// <summary>
        /// 전체 조회 여부를 변경합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_SearchAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox control;
            IBeforeEventArgs beforeEventArgs;
            IDataGridAgent gridAgent;

            try
            {
                control = (CheckBox)sender;

                if (control.Checked)
                    control.Text = "ν";
                else
                    control.Text = "";

                beforeEventArgs = ((IDictionaryForm)this).Dictionary;
                beforeEventArgs.SearchAll = control.Checked;

                gridAgent = this.DataGridAgent(this.DataGridView1);
                //_GridAgent.RemoveColumnFiter(this.txt_Search);
                gridAgent.AddColumnFiter((this.CheckBox_SearchAll.Checked) ? SearchAll.True : SearchAll.False, StartsWith.False, AutoComplete.SuggestAppend, 0, this.TextBox_Search);

                this.TextBox_Search.Focus();

                this.Text_Search_TextChanged(this.TextBox_Search, null);//재검색을 위해서 호출
            }
            catch (Exception exception)
            {
                DiagnosticsTool.MyTrace(exception);
            }
        }

        private void DataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView dataGridView;

            try
            {
                dataGridView = (DataGridView)sender;
                if (dataGridView.ColumnHeadersHeight < e.Y)
                    dataGridView.Tag = new Point(e.X, e.Y);
                else
                    dataGridView.Tag = null;
            }
            catch (Exception exception)
            {
                DiagnosticsTool.MyTrace(exception);
            }
        }

        private void DataGridView1_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView dataGridView;
            Point point;

            try
            {
                dataGridView = (DataGridView)sender;

                if (dataGridView.Tag == null)
                    return;

                if (e.Button != MouseButtons.Left)
                    return;

                point = (Point)dataGridView.Tag;
                dataGridView.FindForm().Left = this.Left + (e.X - point.X);
                dataGridView.FindForm().Top = this.Top + (e.Y - point.Y);
            }
            catch (Exception exception)
            {
                DiagnosticsTool.MyTrace(exception);
            }
        }
        #endregion

        #region "ETC"
        private void SetForm(Form form, DataGridView dataGridView)
        {
            int defaultWidth;
            int defaultHeight;
            int colWidth;
            int rowHeight;
            int scrollWidth;
            int width;
            int height;
            System.Windows.Forms.Control control;
            Point point;
            Rectangle rectangle;
            IDictionary dictionary;

            try
            {
                dictionary = ((IDictionaryForm)form).Dictionary;

                //폼 크기 설정-----------------------------------
                defaultWidth = 25; //최소 폭(폼의 최소 폭)

                if (dataGridView.ColumnHeadersVisible) //최소 높이(검색박스 + 컬럼헤더 높이)
                    defaultHeight = 72 + dataGridView.ColumnHeadersHeight;
                else
                    defaultHeight = 72;

                rowHeight = dataGridView.RowTemplate.Height; //한 행의 높이
                colWidth = 0; //전체 컬럼의 폭
                scrollWidth = 17; //스크롤 넓이

                //DataGridView 폭 가져오기
                foreach (DataGridViewColumn _DataGridViewColumn in dataGridView.Columns)
                    if (_DataGridViewColumn.Visible)
                        colWidth += _DataGridViewColumn.Width;

                form.Size = dictionary.DictionaryFormSize;
                control = dictionary.CurrentControl;

                if (control == null)
                    return;

                point = control.PointToScreen(new Point(0, control.Height));//스크린에서 LOV 컨트롤 위치
                rectangle = Screen.FromPoint(point).Bounds;//스크린 크기

                //If 현재 폭 < (폼 최소 폭 + 전체컬럼 폭 + 스크롤 폭) Or 현재 높이 < (최소높이 + 행 높이) Then
                if (form.Size.Width < (defaultWidth + colWidth + scrollWidth)
                    || form.Size.Height < (defaultHeight + rowHeight)
                    || form.Size.Width > rectangle.Width
                    || form.Size.Height > rectangle.Height)
                {
                    switch (dataGridView.RowCount)
                    {
                        case 0:
                            width = colWidth + defaultWidth;
                            height = defaultHeight;
                            break;
                        case 1:
                            width = colWidth + defaultWidth;
                            height = defaultHeight + rowHeight;//한 행의 높이 만큼 추가
                            break;
                        default:
                            //스크롤이 생기는 거는 폭에 대한 높이의 황금 비율보다 크면 스크롱이 생김
                            //-> 스크롤 포함한 폭에 대한 높이의 황금 비율로 적용
                            width = colWidth + defaultWidth;
                            height = defaultHeight + (rowHeight * dataGridView.RowCount);

                            if (height > width * 1.618)
                            {
                                width += scrollWidth;

                                if (height <= width * 1.618)//스크롤 포함된 폭에 대한 높이의 황금 비율보다 같거나 작으면
                                    width = width - scrollWidth;
                                else
                                    height = (int)(width * 1.618);
                            }
                            break;
                    }

                    if (width > rectangle.Width)
                        width = rectangle.Width;

                    if (height > rectangle.Height)
                        height = rectangle.Height;

                    form.Size = new Size(width, height);
                }

                //폼 위치 설정-----------------------------------
                if (control is TextBox)
                {
                    //_Form 위치 설정
                    //_Point = _Control.PointToScreen(New Point(0, _Control.Height)) '스크린에서 LOV 컨트롤 위치
                    //_Rectangle = Screen.FromPoint(_Point).Bounds '스크린 크기
                    if ((rectangle.X + rectangle.Width) < (point.X + form.Width))//'전체 스크린에서 LOV 컨트롤의 위치가 스크린을 벗어 나면
                        point.X += (rectangle.X + rectangle.Width) - (point.X + form.Width);

                    //If _Point.X < 0 Then
                    //    _Point.X = 0
                    //End If

                    if ((rectangle.Y + rectangle.Height) < (point.Y + form.Height))//전체 스크린에서 LOV 컨트롤의 위치가 스크린을 벗어 나면
                        point.Y += (rectangle.Y + rectangle.Height) - (point.Y + form.Height);

                    //If _Point.Y < 0 Then
                    //    _Point.Y = 0
                    //End If

                    if (form.Opacity != 1)
                    {
                        //_Form.Location = _Point
                        this.SetDesktopLocation(point.X, point.Y);
                    }
                }

                if (form.Opacity != 1)
                    form.Opacity = 1;
            }
            catch (Exception exception)
            {
                DiagnosticsTool.MyTrace(exception);
            }
        }
        #endregion
    }
}
