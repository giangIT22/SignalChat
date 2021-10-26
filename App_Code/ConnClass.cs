using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SignalRChat
{
    public class ConnClass
    {
        public DataTable ExecuteQuery(string query, object[] parameter = null)//excutequery trả về các dòng dữ liệu , dòng kết quả
        {
            DataTable data = new DataTable();
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ToString()))//using dung để giải phóng bộ nhớ
            {
                cnn.Open();//mở kết nối

                SqlCommand cmd = new SqlCommand(query, cnn);//đây sẽ là câu truy vẫn để lấy dữ liệu
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains("@"))
                        {
                            cmd.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);//đi thực thi câu lệnh truy vấn để lấy dữ liệu ra

                adapter.Fill(data); //đổ cái dữ liệu mình dấy ra vào data(bảng dữ liệu)

                cnn.Close();
            }

            return data;
        }


        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ToString()))//using dung để giải phóng bộ nhớ
            {
                cnn.Open();//mở kết nối

                SqlCommand cmd = new SqlCommand(query, cnn);//đây sẽ là câu truy vẫn để lấy dữ liệu
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains("@"))
                        {
                            cmd.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                data = cmd.ExecuteNonQuery();//ExecuteNonQuery() trả về số dòng thành công, chỉ dùng cho insert ,delete,update 
                cnn.Close();
            }

            return data;
        }

    }
}