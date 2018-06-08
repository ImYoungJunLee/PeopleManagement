using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Net.Mail;
using System.Net.Sockets;


namespace DatabaseStudy
{
    class Controller
    {
        static String strConn = "Server=Your IP Address;Database=Your Db Name;Uid=Your DB Id;Pwd=Your Db Password;Charset=utf8";
        MySqlConnection conn = new MySqlConnection(strConn);
        MySqlDataAdapter adapter;
       // DataTable Table= new DataTable();
        Form1 frm1;
        Model M;

        public Controller(Form1 _frm1)
        {
            frm1 = _frm1;
            M = new Model(this);
        }

        public void SELECT()//GridVeiw1에 보여주기
        {
            try
            {
                conn.Open();
                string sql = "SELECT * FROM StudentInfoTb ORDER BY stunum";//Table Name
                adapter = new MySqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                
                adapter.Fill(ds, "StudentInfoTb");
                frm1.DataGridView1.DataSource = ds.Tables[0];
                conn.Close();

            }

            catch (Exception e)
            {
                
                MessageBox.Show(e.ToString());
                conn.Close();
            }
        }

        public void graduate_SELECT()//Gridview2에 보여주기
        {
            try
            {
                conn.Open();
                string sql = "SELECT * FROM StudentInfoTb WHERE state='졸업생' ORDER BY stunum";//학번 정렬
                adapter = new MySqlDataAdapter(sql, conn);
                DataSet ds1 = new DataSet();
                
                adapter.Fill(ds1, "StudentInfoTb");
                frm1.DataGridView2.DataSource = ds1.Tables[0];
                conn.Close();

            }

            catch (Exception e)
            {
               
                MessageBox.Show(e.ToString());
                conn.Close();
            }
        }

        public void student_SELECT()//Gridview3에 보여주기
        {
            try
            {
                conn.Open();
                string sql = "SELECT * FROM StudentInfoTb WHERE state='재학생' ORDER BY stunum";
                adapter = new MySqlDataAdapter(sql, conn);
                DataSet ds2 = new DataSet();
               
                adapter.Fill(ds2, "StudentInfoTb");
                frm1.DataGridView3.DataSource = ds2.Tables[0];
                conn.Close();

            }

            catch (Exception e)
            {
                
                MessageBox.Show(e.ToString());
                conn.Close();
            }
        }

        public void Load_Researcher_List()//불러오기
        {
            try
            {

                conn.Open();
                string sql = "SELECT name,stunum,major,birthday,status,cellphone,email,state FROM StudentInfoTb ORDER BY stunum";
                adapter = new MySqlDataAdapter(sql, conn);
                DataSet ds3 = new DataSet();
               
                adapter.Fill(ds3, "StudentInfoTb");
                frm1.DataGridView1.DataSource = ds3.Tables[0];

                conn.Close();
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Error : " + ex.Message);
                conn.Close();
            }
        }


        public void load_Reseacher_Search()//검색
        {
            try
            {
                conn.Open();
                string sql = "SELECT name,stunum,major,birthday,status,cellphone,email,state FROM StudentInfoTb ORDER BY stunum";
                adapter = new MySqlDataAdapter(sql, conn);
                DataSet ds4 = new DataSet();
           
                adapter.Fill(ds4, "StudentInfoTb");
                DataView DV = new DataView(ds4.Tables[0]);
                
              if (frm1.combo_Box.Text == "이름")
                    DV.RowFilter = string.Format("name LIKE '%" + frm1.Search_Box.Text + "%'");

                else  if (frm1.combo_Box.Text == "학번")
                    DV.RowFilter = string.Format("stunum LIKE '%" + frm1.Search_Box.Text + "%'");

              else
                    DV.RowFilter = string.Format("name LIKE '%" + frm1.Search_Box.Text + "%' OR stunum LIKE '%" + frm1.Search_Box.Text + "%' OR major LIKE '%" + frm1.Search_Box.Text + "%' OR birthday LIKE '%" + frm1.Search_Box.Text + "%' OR status LIKE '%" + frm1.Search_Box.Text + "%' OR cellphone LIKE '%" + frm1.Search_Box.Text + "%' OR email LIKE '%" + frm1.Search_Box.Text + "%' OR state LIKE '%" + frm1.Search_Box.Text + "%'");
                
               frm1.DataGridView1.DataSource = DV;
               frm1.DataGridView1.AutoSize = true;


               conn.Close();
            }
            catch (Exception ex)
            {
           
                MessageBox.Show("Error : " + ex.Message);
                conn.Close();
                
            }
        }

        public void Add_Researcher()//추가
        {
            try
            {
                conn.Open();
                MySqlCommand cmd;
                string sql = "INSERT INTO StudentInfoTb (name, stunum, major, birthday, status, cellphone, email, state) VALUES ('" + frm1.AddName.Text + "', '" + frm1.AddStunum.Text + "', '" + frm1.AddMajor.Text + "', '" + frm1.AddBirthday.Text + "', '" + frm1.AddStatus.Text + "', '" + frm1.AddCellphone.Text + "', '" + frm1.AddEmail.Text + "', '" + frm1.ResearcherCombo.Text +"')";
                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("추가 완료");
                conn.Close();
            }
            catch (Exception ex)
            {
              
                MessageBox.Show("Error : " + ex.Message);
                conn.Close();
            }
            
        }

        public void modify_Researcher()//수정
        {
            conn.Open();
            MySqlCommand com = new MySqlCommand("UPDATE StudentInfoTb SET name = '" + frm1.ModifyName.Text +"', major ='" + frm1.ModifyMajor.Text + "', birthday ='" + frm1.ModifyBirthday.Text + "', status ='" + frm1.ModifyStatus.Text + "', cellphone ='" + frm1.ModifyCellphone.Text + "', email ='" + frm1.ModifyEmail.Text + "', state ='" + frm1.ModifyCombo.Text + "' WHERE (stunum = '" + frm1.ModifyStunum.Text +"')",conn);
            com.ExecuteNonQuery();
            MessageBox.Show("수정 완료");
            conn.Close();

            Load_Researcher_List();

        }

      
       public void delete()//삭제
        {
              //  conn.Open();
                if (MessageBox.Show("선택하신 연구원을 삭제하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
                {
                   // conn.Open();
                    foreach (DataGridViewRow item in frm1.DataGridView1.SelectedRows)
                    {
                        conn.Open();
                        adapter = new MySqlDataAdapter("DELETE FROM StudentInfoTb WHERE (name= '" + item.Cells[0].Value.ToString() + "')", conn);
                        adapter.SelectCommand.ExecuteNonQuery();
                        frm1.DataGridView1.Rows.Remove(item);
                        conn.Close();
                    }
                    MessageBox.Show("삭제 완료", "알림", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                  
                    
                    Load_Researcher_List();
                 
                }
               
            }
     
    
       public void send_to_Registration(int row)//선택한 행의 내용을 수정의 텍스트박스에 넣기
       {
           try
           {
               string combo = frm1.DataGridView1[7, row].Value.ToString();

               frm1.ModifyName.Text = frm1.DataGridView1[0, row].Value.ToString();
               frm1.ModifyStunum.Text = frm1.DataGridView1[1, row].Value.ToString();
               frm1.ModifyMajor.Text = frm1.DataGridView1[2, row].Value.ToString();
               frm1.ModifyBirthday.Text = frm1.DataGridView1[3, row].Value.ToString();
               frm1.ModifyStatus.Text = frm1.DataGridView1[4, row].Value.ToString();
               frm1.ModifyCellphone.Text = frm1.DataGridView1[5, row].Value.ToString();
               frm1.ModifyEmail.Text = frm1.DataGridView1[6, row].Value.ToString();
               frm1.ModifyCombo.SelectedItem = combo;
               
           }
           catch (Exception e)
           {

               MessageBox.Show(e.ToString());
               
           }
         
          
       }
       
       public void SendMail()
       {
           if (MessageBox.Show("이메일을 발송하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
           {
               MailAddress mailFrom = new MailAddress(M.MAIL_ID, M.MAIL_ID_NAME, Encoding.UTF8); // 보내는사람의 정보를 생성
               MailAddress mailTo = new MailAddress(frm1.ReceiverRichBox.Text); // 받는사람의 정보를 생성
               SmtpClient client = new SmtpClient(M.SMTP_SERVER, M.SMTP_PORT); // smtp 서버 정보를 생성
               MailMessage message = new MailMessage(mailFrom, mailTo);

               message.Subject = frm1.SubjectTextBox.Text; // 메일 제목 프로퍼티

               message.Body = frm1.BodyRichBox.Text; // 메일의 몸체 메세지 프로퍼티

               message.BodyEncoding = Encoding.UTF8; // 메세지 인코딩 형식

               message.SubjectEncoding = Encoding.UTF8; // 제목 인코딩 형식

               client.EnableSsl = true; // SSL 사용 유무 (네이버는 SSL을 사용합니다. )

               client.DeliveryMethod = SmtpDeliveryMethod.Network;

               client.Credentials = new System.Net.NetworkCredential(M.MAIL_ID, M.MAIL_PW); // 보안인증 ( 로그인 )

               client.Send(message);  //메일 전송 

               message.Dispose();

               MessageBox.Show("전송 완료");

               frm1.reset_Email();
           }

        }

/*
       public void SendAllMail()
       {
           if (MessageBox.Show("이메일을 발송하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
           {
               for (int i = 0; i < frm1.DataGridView1.Rows.Count; i++)
               {
                   MailAddress mailFrom = new MailAddress(M.MAIL_ID, M.MAIL_ID_NAME, Encoding.UTF8); // 보내는사람의 정보를 생성
                  String email = frm1.DataGridView1.Rows[i].Cells[6].Value.ToString();
                   if (email != "" && email != null)
                   {

                       MailAddress mailTo = new MailAddress(email); // 받는사람의 정보를 생성
                       SmtpClient client = new SmtpClient(M.SMTP_SERVER, M.SMTP_PORT); // smtp 서버 정보를 생성
                       MailMessage message = new MailMessage(mailFrom, mailTo);

                       message.Subject = frm1.SubjectTextBox.Text; // 메일 제목 프로퍼티

                       message.Body = frm1.BodyRichBox.Text; // 메일의 몸체 메세지 프로퍼티

                       message.BodyEncoding = Encoding.UTF8; // 메세지 인코딩 형식

                       message.SubjectEncoding = Encoding.UTF8; // 제목 인코딩 형식

                       client.EnableSsl = true; // SSL 사용 유무 (네이버는 SSL을 사용합니다. )

                       client.DeliveryMethod = SmtpDeliveryMethod.Network;

                       client.Credentials = new System.Net.NetworkCredential(M.MAIL_ID, M.MAIL_PW); // 보안인증 ( 로그인 )

                       client.Send(message);  //메일 전송 

                       message.Dispose();

                       MessageBox.Show("전송 완료");

                       frm1.reset_Email();

                   }
               }
          }
       }*/

       public void send_to_Email(int row)//선택한 행의 내용을 이메일의 텍스트박스에 넣기
       {
           try
           {
               frm1.ReceiverRichBox.Text = frm1.DataGridView1[6, row].Value.ToString();
           }

           catch (Exception e)
           {

               MessageBox.Show(e.ToString());
              
           }
       }
    }
}
