using System.Text;
using System.Net.Sockets;
using System.Web.Script.Serialization;

namespace ClassLibrary1
{
    public class Channel
    {
        public Socket Socket;

        public Channel(Socket socket)
        {
            Socket = socket;
        }

        public void Send(TodoList list)
        {
            list.PrintAll();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonList = serializer.Serialize(list);
            byte[] message = Encoding.Unicode.GetBytes(jsonList);
            Socket.Send(message);
        }

        public TodoList Receive()
        {
            StringBuilder builder = new StringBuilder();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            byte[] buffer = new byte[256];
            int bytes = 0;
            do
            {
                bytes = Socket.Receive(buffer);
                builder.Append(Encoding.Unicode.GetString(buffer, 0, bytes));
            } while (Socket.Available > 0);
            string jsonList = builder.ToString();
            TodoList list = serializer.Deserialize<TodoList>(jsonList);
            return list;
        }
    }
}
