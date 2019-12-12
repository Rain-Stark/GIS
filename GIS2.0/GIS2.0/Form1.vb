Imports System.IO
Public Class Form1
    Dim x As Single = 0.0F
    Dim y As Single = 0.0F
    Dim Coord(1000) As PointF
    Dim i As Integer = 0
    Dim g As Graphics
    Dim sandyPen As New Pen(Color.SandyBrown, 1)
    Dim s As Single = 0.4F
    Dim p_s As Single = 0.4F
    Dim xx As Single
    Dim yy As Single
    Dim t_x As Single = 0.0F
    Dim t_y As Single = 0.0F
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
    Private Sub Draw()
        x = Me.Width
        y = Me.Height
        Dim str, str1(1) As String
        Dim sr As StreamReader = New StreamReader("coastline.dat", System.Text.Encoding.Default)
        g = Me.CreateGraphics
        g.TranslateTransform(x / 2.0F + t_x * s, y / 2.0F + t_y * s)
        g.Clear(Me.BackColor)
        Do While sr.Peek() > 0
            str = sr.ReadLine
            str1 = Split(str, "	")
            If str <> "# -b" Then
                If str1(0) <> "171.072547  8.763082" Then
                    Coord(i).X = Single.Parse(str1(0)) * s / 180.0F * x
                    Coord(i).Y = Single.Parse(str1(1)) * s / 90.0F * y * -1.0F
                Else
                    Coord(i).X = 171.07254F * s / 180.0F * x
                    Coord(i).Y = 8.763082F * s / 90.0F * y * -1.0F
                End If
                i += 1
            ElseIf str = "# -b" Then
                If i <> 0 Then
                    For j = i - 1 To 1000
                        Coord(j).X = Coord(i - 1).X
                        Coord(j).Y = Coord(i - 1).Y
                    Next
                    g.DrawLines(sandyPen, Coord)
                    'g.DrawPath()
                    i = 0
                End If
            End If
        Loop
        sr.Close()
        sr = Nothing
    End Sub
    Private Sub Form1_MouseMove1(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        xx = (e.Location.X - x / 2.0F - t_x * s) / x * 180.0F / s
        yy = (e.Location.Y - y / 2.0F - t_y * s) * -1.0F / y * 90.0F / s
        If Math.Abs(yy) <= 90.0 And Math.Abs(xx) <= 180.0 Then
            Me.ToolStripStatusLabel1.Text = "纬度" + yy.ToString("#0.00") + "   经度" + xx.ToString("#0.00") '文本框的内容为X坐标和Y坐标
        Else
            Me.ToolStripStatusLabel1.Text = "经纬度"
        End If
    End Sub
    Private Sub Form1_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel

        If e.Delta > 0 And s >= 0.4F * 1.2F Then
            s = s / 1.2F
            t_x += (e.X - x / 2.0F) / p_s / 4.0F
            t_y += (e.Y - y / 2.0F) / p_s / 4.0F
        ElseIf e.Delta < 0 And s * 1.2F < 8.0F Then
            s = s * 1.2F
            t_x -= (e.X - x / 2.0F) / s / 4.0F
            t_y -= (e.Y - y / 2.0F) / s / 4.0F
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If x <> Me.Width Then
            Draw()
        End If
        If s <> p_s Then
            Draw()
            p_s = s
        End If
    End Sub
    Dim u_x As Single
    Dim u_y As Single
    Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        d_x = e.X
        d_y = e.Y
    End Sub
    Dim d_x As Single
    Dim d_y As Single
    Private Sub Form1_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        u_x = e.X
        u_y = e.Y
        t_x += (u_x - d_x) / s
        t_y += (u_y - d_y) / s
        Draw()
    End Sub
End Class