using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Threading;
using System.Net.Sockets;

namespace DatabaseStudy
{
    public partial class Form1 : Form
    {
        Controller control;
       

        public Form1()
        {
            InitializeComponent();
            control = new Controller(this);
            DataGridView1.EnableHeadersVisualStyles = false;
            DataGridView2.EnableHeadersVisualStyles = false;
            DataGridView3.EnableHeadersVisualStyles = false;
        }

        private void ResearcherList_Click(object sender, EventArgs e)//연구원 목록 버튼
        {
            tabControl1.SelectedIndex = 1;          
            control.SELECT();
            control.student_SELECT();
            control.graduate_SELECT();
           // Search_btn.Enabled = true;//검색버튼 활성화
           // Search_Box.Enabled = true;
            
        }



        private void Search_btn_Click(object sender, EventArgs e)//검색 버튼 눌렀을때
        {
            if (Search_Box.Text == "")
                MessageBox.Show("검색어를 입력해주세요", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                DataGridView1.Visible = true;
                tabControl1.SelectedIndex = 1;
                control.load_Reseacher_Search();//comboBox의 조건이 있을 시까지 포함한 검색
                Search_Box.Text = "";
            }

        }

        private void Search_keyDown(object sender, KeyEventArgs e)//검색창에 키를 입력하였을때
        {
            if (e.KeyCode == Keys.Enter)
                Search_btn_Click(null, null);//엔터 누르면 검색버튼 불러오기

          /*  else if (e.KeyCode == Keys.Back)//백스페이스 키
            {
                if (Search_Box.Text.Length <= 1)//검색창의 글자 길이가 없을떄
                    Load_Book_List();
            }*/
        }

        private void ResearcherEnrollbtn_Click(object sender, EventArgs e)//등록 버튼
        {
            control.Add_Researcher();
            control.Load_Researcher_List();
            control.graduate_SELECT();//등록하는 순간에 DataGridVeiw2도 업데이트
            control.student_SELECT();
            ResearcherReinputbtn_Click(null, null);
        }

        private void ResearcherReinputbtn_Click(object sender, EventArgs e)
        {
            AddName.Text = "";
            AddStunum.Text = "";
            AddMajor.Text = "";
            AddBirthday.Text = "";
            AddStatus.Text = "";
            AddCellphone.Text = "";
            AddEmail.Text = "";
            ResearcherCombo.Text = "";
        }

        private void ResearcherModifybtn_Click(object sender, EventArgs e)//수정 버튼
        {
            control.modify_Researcher();
            ModifyReinputbtn_Click(null,null);
        }

        public void reset_Modify()
        {
            ModifyName.Text = "";
            ModifyStunum.Text = "";
            ModifyMajor.Text = "";
            ModifyBirthday.Text = "";
            ModifyStatus.Text = "";
            ModifyCellphone.Text = "";
            ModifyEmail.Text = "";
            ModifyCombo.Text = "";
        }

        private void ModifyReinputbtn_Click(object sender, EventArgs e)
        {
            ModifyName.Text = "";
            ModifyStunum.Text = "";
            ModifyMajor.Text = "";
            ModifyBirthday.Text = "";
            ModifyStatus.Text = "";
            ModifyCellphone.Text = "";
            ModifyEmail.Text = "";
            ModifyCombo.Text = "";
        }

        private void ResearcherDeletebtn_Click(object sender, EventArgs e)//삭제 버튼
        {
            control.delete();
            control.graduate_SELECT();//등록하는 순간에 DataGridVeiw2도 업데이트
            control.student_SELECT();
           // control.default_in_Client();
            reset_Modify();
        }

        private void DataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)//DataGridVeiw1테이블 누르면 수정 Text에 넣기
        {
            //tabControl2.SelectedTab = tabModify;
            control.send_to_Registration(e.RowIndex);//Not +1
            control.send_to_Email(e.RowIndex);
        }

        private void ReturnResearcherListbtn_Click(object sender, EventArgs e)//목록으로 돌아가기
        {
            ResearcherList_Click(null,null);
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == tabMain)
            {
                Search_Group.Enabled = false;
                tabControl2.Enabled = false;
                ResearcherDeletebtn.Enabled = false;//삭제버튼 활성화

            }

            else if (e.TabPage == tabAll)
            {
                Search_Group.Enabled = true;
                tabControl2.Enabled = true;
                ResearcherDeletebtn.Enabled = true;//삭제버튼 활성화
            }

            else if (e.TabPage == tabStudent)
            {
                Search_Group.Enabled = true;
                tabControl2.Enabled = true;
                ResearcherDeletebtn.Enabled = true;//삭제버튼 활성화
            }

            else if (e.TabPage == tabGraduate)
            {
                Search_Group.Enabled = true;
                tabControl2.Enabled = true;
                ResearcherDeletebtn.Enabled = true;//삭제버튼 활성화
            }
        }

        private void SendEmailbtn_Click(object sender, EventArgs e)//이메일 발송
        {
            /*Thread myThread;
            myThread = new Thread(new ThreadStart(control.SendMail));
            myThread.Start();//메일 발송되는 동안 스레딩*/

            try
            {
                control.SendMail();

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
        }

        public void reset_Email()
        {
            ReceiverRichBox.Text = "";
            SubjectTextBox.Text = "";
            BodyRichBox.Text = "";
            
        }

        private void EmailReinputbtn_Click(object sender, EventArgs e)
        {
            reset_Email();
        }

        private void SendAllbtn_Click(object sender, EventArgs e)
        {
            /*Thread myThread;
            myThread = new Thread(new ThreadStart(control.SendAllMail));
            myThread.Start();//메일 발송되는 동안 스레딩
            */

            try
            {
                control.SendAllMail();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void EmailReinputbtn_Click_1(object sender, EventArgs e)
        {
            reset_Email();
        }

        private void DataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)//행번호 나타내주기
        {
            Rectangle rect = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y,DataGridView1.RowHeadersWidth - 4,e.RowBounds.Height);
           
            // 위에서 생성된 장방형내에 행번호를 보여주고 폰트색상 및 배경을 설정
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),DataGridView1.RowHeadersDefaultCellStyle.Font,
            rect,DataGridView1.RowHeadersDefaultCellStyle.ForeColor,TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void DataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rect = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, DataGridView1.RowHeadersWidth - 4, e.RowBounds.Height);

            // 위에서 생성된 장방형내에 행번호를 보여주고 폰트색상 및 배경을 설정
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), DataGridView2.RowHeadersDefaultCellStyle.Font,
            rect, DataGridView2.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void DataGridVeiw3_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rect = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, DataGridView1.RowHeadersWidth - 4, e.RowBounds.Height);

            // 위에서 생성된 장방형내에 행번호를 보여주고 폰트색상 및 배경을 설정
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), DataGridView3.RowHeadersDefaultCellStyle.Font,
            rect, DataGridView3.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void ModifyName_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    
    }
}
    