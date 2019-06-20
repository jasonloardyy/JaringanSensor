Imports System.IO.Ports
Imports System
Imports System.Net
Imports System.IO

Public Class Form1

    Dim comPORT As String
    Dim receivedData As String = ""

    Dim temp As String
    Dim humidity As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Enabled = False
        comPORT = ""
        For Each sp As String In My.Computer.Ports.SerialPortNames
            ComboBox1.Items.Add(sp)
        Next
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If (ComboBox1.SelectedItem <> "") Then
            comPORT = ComboBox1.SelectedItem
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If (Button1.Text = "Connect") Then
            Try
                If (comPORT <> "") Then
                    SerialPort1.Close()
                    SerialPort1.PortName = comPORT
                    SerialPort1.BaudRate = 9600
                    SerialPort1.DataBits = 8
                    SerialPort1.Parity = Parity.None
                    SerialPort1.StopBits = StopBits.One
                    SerialPort1.Handshake = Handshake.None
                    SerialPort1.Encoding = System.Text.Encoding.Default
                    SerialPort1.ReadTimeout = 10000

                    SerialPort1.Open()
                    Button1.Text = "Disconnect"
                    Timer1.Enabled = True
                Else
                    MsgBox("Pilih COM Port!")
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Else
            SerialPort1.Close()
            Button1.Text = "Connect"
            Timer1.Enabled = False
        End If
    End Sub

    Function ReceiveSerialData() As String
        Dim Incoming As String
        Try
            Incoming = SerialPort1.ReadExisting()
            If Incoming Is Nothing Then
                Return "nothing" & vbCrLf
            Else
                Return Incoming
            End If
        Catch ex As TimeoutException
            Return "Error: Serial Port read timed out."
        End Try

    End Function

    Sub PostThingspeak()
        If CheckBox1.Checked = True Then
            Dim request As HttpWebRequest = HttpWebRequest.Create("https://api.thingspeak.com/update?api_key=QRSUHLQKDCKWZRE4&field1=" & temp & "&field2=" & humidity)
            Dim response As HttpWebResponse = request.GetResponse
            Dim reader As New StreamReader(response.GetResponseStream)
            Dim strings As String = reader.ReadToEnd
            If strings <> "0" Then
                TextBox3.Text = strings
                TextBox4.Text = "1"
            Else
                TextBox4.Text = "0"
            End If
        End If
    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        receivedData = ReceiveSerialData()
        If receivedData.Contains("Temp : ") And receivedData.Contains(" C") And receivedData.Contains("Humidity : ") And receivedData.Contains(" %") Then
            temp = receivedData.Substring(7, 2)
            humidity = receivedData.Substring(27, 2)
            If IsNumeric(temp) And IsNumeric(humidity) Then
                TextBox1.Text = temp
                TextBox2.Text = humidity
                PostThingspeak()
            End If
            If TextBox4.Text = "1" Then
                RichTextBox1.AppendText("==============================" + vbCrLf + receivedData + "Berhasil ter-kirim!" + vbCrLf + "==============================" + vbCrLf)
            Else
                RichTextBox1.AppendText("==============================" + vbCrLf + receivedData + "==============================" + vbCrLf)
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        RichTextBox1.Text = ""
    End Sub

    Private Sub RichTextBox1_TextChanged(sender As Object, e As EventArgs) Handles RichTextBox1.TextChanged
        RichTextBox1.SelectionStart() = RichTextBox1.TextLength
        RichTextBox1.ScrollToCaret()
    End Sub
End Class
