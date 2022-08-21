Imports System.Net
Imports System.Net.Sockets
Imports System.Text.RegularExpressions


'│ Author       : suXjung
'│ Name         : Port Checker and Hostname Resolver
'│ Tel          : @sujung02 

'This program Is distributed For educational purposes only.


Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Opacity = 0.92
        TextBox1.Text = GetExternalIp().ToString
    End Sub

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        MessageBox.Show(Me, vbCrLf & "       │ Author         suXjung" & vbCrLf & "       │ Name          Net Tool © 2021" & vbCrLf & "       │ Tel              @sujung02" & vbCrLf, "Net Tool", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
    End Sub


    Private Function IsPortOpen(ByVal Host As String, ByVal PortNumber As Integer) As Boolean
        Dim Client As TcpClient = Nothing
        Try
            Client = New TcpClient(Host, PortNumber)
            Return True
        Catch ex As SocketException
            Return False
        Finally
            If Not Client Is Nothing Then
                Client.Close()
            End If
        End Try
    End Function

    Public Function GetExternalIp() As String
        Try
            Dim ExternalIP As String
            ExternalIP = (New WebClient()).DownloadString("http://checkip.dyndns.org/")
            ExternalIP = (New Regex("\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")) _
                     .Matches(ExternalIP)(0).ToString()
            Return ExternalIP
        Catch
            Return Nothing
        End Try
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Then
            MessageBox.Show("Enter a valid IP address!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        Dim PortOpen As Boolean = IsPortOpen(TextBox1.Text, NumericUpDown1.Value.ToString)
        Label3.Visible = True
        If PortOpen = True Then
            Label3.Text = "Success"
            Label3.ForeColor = Color.Green
            TextBox2.Text = "Service at " & TextBox1.Text & " is running, port " & NumericUpDown1.Value.ToString & " is open."
        Else
            Label3.Text = "Error"
            Label3.ForeColor = Color.Red
            TextBox2.Text = "Service at " & TextBox1.Text & " is not running or port " & NumericUpDown1.Value.ToString & " is closed."
        End If
    End Sub



    Public Function GetHostNameFromIP(ByRef IP As String) As String
        Try
            Dim host As IPHostEntry = Dns.GetHostEntry(IP.Trim())
            Dim ipaddr As IPAddress() = host.AddressList
            ' Loop through the IP Address array and add the IP address to Listbox
            For Each addr As IPAddress In ipaddr

                TextBox4.Text += addr.ToString() & Environment.NewLine

            Next addr
            ' Catch unknown host names
        Catch ex As System.Net.Sockets.SocketException
            MessageBox.Show(ex.Message, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As System.Exception
            MessageBox.Show(ex.Message, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox3.Text = "" Then
            MessageBox.Show("Enter a valid IP address!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            GetHostNameFromIP(TextBox3.Text)
        End If
    End Sub
End Class
