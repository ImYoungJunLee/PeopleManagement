using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DatabaseStudy
{
    class Model
    {
        Controller control;
        public Model(Controller _control)
        {
            control = _control;

        }

        public string SMTP_SERVER = "smtp.naver.com"; // SMTP 서버 주소
        public int SMTP_PORT = 587; // SMTP 포트
        public string MAIL_ID = "Your email"; // ex) xxxx@naver.com
        public string MAIL_ID_NAME = "Your Id"; // naver,google...Id
        public string MAIL_PW = "Your Password";  // Id's password

    }
}
