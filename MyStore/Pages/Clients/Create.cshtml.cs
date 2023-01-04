using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
   
    public class CreateModel : PageModel
    {
        //initializing Error and success message of the project

        public ClientInfo ClientInfo = new ClientInfo();
        public string errorMessage = "";
        public string successMessge = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            ClientInfo.Name = Request.Form["name"];
            ClientInfo.Email = Request.Form["email"];
            ClientInfo.Phone = Request.Form["phone"];
            ClientInfo.Address = Request.Form["address"];

            if (ClientInfo.Name.Length == 0 || ClientInfo.Email.Length == 0 ||
                ClientInfo.Phone.Length == 0 || ClientInfo.Address.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            //Save the New client into the database
            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=MyStore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO Clients" +
                                 "(name, email, phone, address) VALUES" +
                                 "(@name, @email, @phone, @address);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", ClientInfo.Name);
                        command.Parameters.AddWithValue("@email", ClientInfo.Email);
                        command.Parameters.AddWithValue("@phone", ClientInfo.Phone);
                        command.Parameters.AddWithValue("@address", ClientInfo.Address);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            //Calling the success message to confirm the Added client
            ClientInfo.Name = ""; ClientInfo.Email = ""; ClientInfo.Phone = ""; ClientInfo.Address = "";
            successMessge = "New Client Added Successfully";

            Response.Redirect("/Clients/Index");
        }
    }
}
