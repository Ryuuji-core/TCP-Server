using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace Testサーバーシステム
{
    class Program
    {
        static void Main(string[] args)
        {
            //ListenするIPアドレスを決めます。
            string host = "localhost";
            System.Net.IPAddress ipAdd = System.Net.Dns.GetHostEntry(host).AddressList[0];
            //Lintenするポート
            int port = 2001;
            //TcpListener ←オブジェクト
            System.Net.Sockets.TcpListener listener = new System.Net.Sockets.TcpListener(ipAdd, port);
            //Lintenを開始します。
            listener.Start();
            Console.WriteLine("Listenを開始しました。({0}:{1})",
                ((System.Net.IPEndPoint)listener.LocalEndpoint).Address,
                ((System.Net.IPEndPoint)listener.LocalEndpoint).Port
                );
            //要求があったら受信する(受ける)。
            System.Net.Sockets.TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("クライアント({0}:{1})と接続しました。「接続完了」",
                ((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Address,
                ((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Port
                );
            //NetStreamを取得。
            System.Net.Sockets.NetworkStream ns = client.GetStream();
            //クライアントから送信されたデーターを受信する。
            System.Text.Encoding enc = System.Text.Encoding.UTF8;
            bool disconnected = false;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            byte[] resBytes = new byte[256];
            do
            {
                //データの一部を受信する
                int resSize = ns.Read(resBytes, 0, resBytes.Length);
                //Readが0を返した時はクライアントが切断したと判断
                if (resSize == 0)
                {
                    disconnected = true;
                    Console.WriteLine("クライアントが切断しました。");
                    break;
                }
                else if (resSize == 0)
                {
                    disconnected = false;
                    Console.WriteLine("クライアントが切断しました。");
                    break;
                }
                else
                {
                    disconnected = true;
                    Console.WriteLine("クライアントが切断しました。");
                    break;
                }
                ms.Write(resBytes, 0, resSize);
            } while (ns.DataAvailable);
            //受信したデーターを文字列に変換。
            string resMsg = enc.GetString(ms.ToArray());
            ms.Close();
            Console.WriteLine(resMsg);
            if (!disconnected)
            {
                //クライアントにデータを送信します。
                //クライアントに送信する文字列を作成します。""。
             string sendMsg = resMsg.Length.ToString();
            //文字列をByte型配列に変換
            byte[] sendBytes = enc.GetBytes(sendMsg);
            //データを送信する
            ns.Write(sendBytes, 0, sendBytes.Length);
            Console.WriteLine(sendMsg);
            }
            else
            {
                //クライアントにデータを送信します。
                //クライアントに送信する文字列を作成します。""。
                string sendMsg = resMsg.Length.ToString();
                //文字列をByte型配列に変換
                byte[] sendBytes = enc.GetBytes(sendMsg);
                //データを送信する
                ns.Write(sendBytes, 0, sendBytes.Length);
                Console.WriteLine(sendMsg);
            }
            //閉　クライアントとの接続を閉じる。
            ns.Close();
            client.Close();
            Console.WriteLine("クライアントとの接続を切断しました。");
            //リスナを閉じる
            ns.Close();
            client.Close();
            Console.WriteLine("Listenerを閉じました。");
            Console.ReadLine();
        }
    }
}
