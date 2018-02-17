Imports System.Windows
Imports System.Math
Public Class Form1

    Dim ddx, y2, z2, K1, K2, K3, K4, D1, D2, D3, D4 As Double
    ''Runge Kuta 計算用變數
    Dim dt = 0.1
    Dim t As Double
    Dim last_t As Double
    Dim m As Double
    Dim c As Double
    Dim k As Double
    Dim L           '' 彈簧原長
    Dim g = 9.81
    '' 物理變數
    Dim last_Y2
    Dim wheel_dY2, wheel_Y2 As Double
    ''輪子位置速度
    Dim Y1, dY1
    '' 彈簧和阻尼上方位置速度
    Dim originY, originX ''輪子圖片左上角Y方向原點

    Dim obsticle_Type As Single
    Dim none_rock, small_single_rock, small_rock, big_hill
    Dim rock_count
    Dim plot_t

    Dim Animation_speed 'Timer rate
    Dim outputData(2, 400) As Double
    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        initial()
    End Sub
    'Function Runge(t, y1, z1)

    '    K1 = dt * z1
    '    D1 = dt * f(t, y1, z1)
    '    K2 = dt * (z1 + D1 / 2)
    '    D2 = dt * f(t + dt / 2, y1 + K1 / 2, z1 + D1 / 2)
    '    K3 = dt * (z1 + D2 / 2)
    '    D3 = dt * f(t + dt / 2, y1 + K2 / 2, z1 + D2 / 2)
    '    K4 = dt * (z1 + D3)
    '    D4 = dt * f(t + dt, y1 + K3, z1 + D3)

    '    y2 = y1 + (K1 + 2 * K2 + 2 * K3 + K4) / 6
    '    z2 = z1 + (D1 + 2 * D2 + 2 * D3 + D4) / 6
    'End Function
    Function f(t, y1, dy1) As Double
        ddx = g - (c * (dy1 - wheel_dY2) + k * (y1 - wheel_Y2)) / m
        Return ddx
    End Function
    Function obsticle_f()

        Select Case obsticle_Type
            Case none_rock
                wheel_Y2 = 0
                wheel_dY2 = 0
            Case small_single_rock
                If t >= 1.4 And t <= 2.6 Then
                    wheel_Y2 = 25 * (t - 2) * (t - 2) - 9
                    wheel_dY2 = 50 * (t - 2) * t
                Else
                    wheel_Y2 = 0
                    wheel_dY2 = 0
                End If
            Case small_rock
                If t - 1.8 * rock_count >= 0.5 And t - 1.8 * rock_count <= 1.5 Then
                    wheel_Y2 = 36 * (t - 1.8 * rock_count - 1) * (t - 1.8 * rock_count - 1) - 9
                    wheel_dY2 = 72 * (t - 1.8 * rock_count - 1) * (t - 1.8 * rock_count)
                Else
                    wheel_Y2 = 0
                    wheel_dY2 = 0
                End If
                If t - 1.8 * rock_count > 1.8 Then
                    rock_count += 1
                End If
            Case big_hill
                If t >= 1 And t <= 3 Then
                    wheel_Y2 = 25 * (t - 2) * (t - 2) - 25
                    wheel_dY2 = 50 * (t - 2) * t
                Else
                    wheel_Y2 = 0
                    wheel_dY2 = 0
                End If
        End Select

    End Function
    Function draw_animation()
        PictureBox2.Location() = New Point(originX, Y1 + originY - L)
        PictureBox2.Height() = (wheel_Y2 + originY) - (Y1 + originY - L)
        PictureBox3.Location() = New Point(originX + 10, (wheel_Y2 + originY) - PictureBox3.Height() / 2 + 10)
    End Function
    Function draw_plot()
        Dim g As Graphics = PictureBox1.CreateGraphics()
        'g.FillRectangle(Brushes.Red, 140, 40, 1, 1)
        Dim g2 As Graphics = PictureBox1.CreateGraphics()

        g.DrawLine(Pens.Red, CInt(10 * (plot_t - dt)), 30 + CInt(Y1), CInt(10 * plot_t), 30 + CInt(y2))
        g2.DrawLine(Pens.Blue, CInt(10 * (plot_t - dt)), 80 + CInt(last_Y2), CInt(10 * plot_t), 80 + CInt(wheel_Y2))
        outputData(0, CInt(10 * (plot_t - dt))) = Y1
        If plot_t * 10 >= PictureBox1.Size.Width Then
            plot_t = 0
            PictureBox1.Refresh()
        End If
    End Function
    Function initial()
        m = 100
        c = 15
        k = 50
        t = 0
        last_t = 0
        plot_t = 0
        K1 = 0
        K2 = 0
        K3 = 0
        K4 = 0
        D1 = 0
        D2 = 0
        D3 = 0
        D4 = 0
        Y1 = 10
        dY1 = 0
        wheel_dY2 = 0
        y2 = 0
        last_Y2 = y2
        L = 150
        originX = 100
        originY = 300

        none_rock = 0
        small_single_rock = 1
        small_rock = 2
        big_hill = 3
        obsticle_Type = small_single_rock
        rock_count = 0

        Animation_speed = 50
        Timer1.Interval = Animation_speed
        Timer1.Enabled = False

        HScrollBar1.Maximum = 500         '' tune the mass 
        HScrollBar1.Minimum = 0
        HScrollBar1.Value = m
        NumericUpDown1.Maximum = 500
        NumericUpDown1.Minimum = 0
        NumericUpDown1.Value = m

        HScrollBar2.Maximum = 500         '' tune the damp 
        HScrollBar2.Minimum = 0
        HScrollBar2.Value = c
        NumericUpDown2.Maximum = 500
        NumericUpDown2.Minimum = 0
        NumericUpDown2.Value = c

        HScrollBar3.Maximum = 500     '' tune the spring
        HScrollBar3.Minimum = 0
        HScrollBar3.Value = k
        NumericUpDown3.Maximum = 500
        NumericUpDown3.Minimum = 0
        NumericUpDown3.Value = k

        HScrollBar4.Maximum = 100        '' tune the animation rate 
        HScrollBar4.Minimum = 10
        HScrollBar4.Value = Animation_speed
        draw_animation()


        ErrorProvider1.Clear()
    End Function
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Start.Click
       
        t = 0
        plot_t = 0
        Timer1.Enabled() = True
        PictureBox1.Refresh()
        If m / k > L - 50 Then
            Timer1.Enabled() = False
        End If
        'Do
        '    t = t + dt
        '    Label1.Text = t

        '    Runge(t, x, dx)

        '    draw_animation()
        '    draw_plot()

        '    x = y2
        '    dx = z2
        '    last_x2 = x2

        '    Application.DoEvents()

        'Loop
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles NumericUpDown1.ValueChanged
        m = NumericUpDown1.Value
        HScrollBar1.Value = NumericUpDown1.Value
        If m = 0 Then
            m = 1
            NumericUpDown1.Value = m
            HScrollBar1.Value = NumericUpDown1.Value
        End If
        If m / k >= L Then
            Timer1.Enabled = False
            ErrorProvider1.SetError(NumericUpDown1, "M is too big. K is too small")
        Else
            ErrorProvider1.Clear()
        End If

    End Sub

    Private Sub HScrollBar1_Scroll(sender As System.Object, e As System.Windows.Forms.ScrollEventArgs) Handles HScrollBar1.Scroll
        NumericUpDown1.Value = HScrollBar1.Value
        m = NumericUpDown1.Value
        If m = 0 Then
            m = 1
            NumericUpDown1.Value = m

        End If
        If m / k >= L Then
            Timer1.Enabled = False
            ErrorProvider1.SetError(NumericUpDown1, "M is too big. K is too small")
        Else
            ErrorProvider1.Clear()
        End If

    End Sub

    Private Sub HScrollBar2_Scroll(sender As System.Object, e As System.Windows.Forms.ScrollEventArgs) Handles HScrollBar2.Scroll
        NumericUpDown2.Value = HScrollBar2.Value
        c = NumericUpDown2.Value
    End Sub

    Private Sub NumericUpDown2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles NumericUpDown2.ValueChanged
        c = NumericUpDown2.Value
    End Sub

    Private Sub HScrollBar3_Scroll(sender As System.Object, e As System.Windows.Forms.ScrollEventArgs) Handles HScrollBar3.Scroll
        NumericUpDown3.Value = HScrollBar3.Value
        k = NumericUpDown3.Value
        If m / k >= L Then
            Timer1.Enabled = False
            ErrorProvider1.SetError(NumericUpDown3, "M is too big. K is too small")
        Else
            ErrorProvider1.Clear()
        End If
    End Sub

    Private Sub NumericUpDown3_ValueChanged(sender As System.Object, e As System.EventArgs) Handles NumericUpDown3.ValueChanged
        k = NumericUpDown3.Value
        If m / k >= L Then
            Timer1.Enabled = False
            ErrorProvider1.SetError(NumericUpDown3, "M is too big. K is too small")
        Else
            ErrorProvider1.Clear()
        End If
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick

        Label1.Text = t

        obsticle_f()

        K1 = dt * dY1                   ''dY1 速度    ''f() 加速度   Y1
        D1 = dt * f(t, Y1, dY1)
        K2 = dt * (dY1 + D1 / 2)
        D2 = dt * f(t + dt / 2, Y1 + K1 / 2, dY1 + D1 / 2)
        K3 = dt * (dY1 + D2 / 2)
        D3 = dt * f(t + dt / 2, Y1 + K2 / 2, dY1 + D2 / 2)
        K4 = dt * (dY1 + D3)
        D4 = dt * f(t + dt, Y1 + K3, dY1 + D3)

        y2 = Y1 + (K1 + 2 * K2 + 2 * K3 + K4) / 6
        z2 = dY1 + (D1 + 2 * D2 + 2 * D3 + D4) / 6
        Y1 = y2
        dY1 = z2

        t = t + dt
        plot_t = plot_t + dt
        draw_animation()
        Label2.Text = Y1
        Label3.Text = dY1
        Label4.Text = f(t, Y1, dY1)
        draw_plot()
        last_Y2 = wheel_Y2

      
        If t >= 300 Then  '' 自動停止
            Timer1.Enabled = False
        End If
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Timer1.Enabled = False
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Reset.Click
        initial()
    End Sub

    Private Sub Button1_Click_1(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        obsticle_Type = small_rock
        t = 0
        plot_t = 0
        rock_count = 0
        PictureBox1.Refresh()
    End Sub
    Private Sub Button3_Click_1(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        obsticle_Type = big_hill
        t = 0
        plot_t = 0
        PictureBox1.Refresh()
    End Sub
    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        obsticle_Type = small_single_rock
        t = 0
        plot_t = 0
        PictureBox1.Refresh()
    End Sub
    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        obsticle_Type = none_rock
        t = 0
        plot_t = 0
        PictureBox1.Refresh()
    End Sub
    Private Sub HScrollBar4_Scroll(sender As System.Object, e As System.Windows.Forms.ScrollEventArgs) Handles HScrollBar4.Scroll
        Animation_speed = HScrollBar4.Value
        Timer1.Interval() = Animation_speed
    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click
        SaveFileDialog1.Filter() = "文字檔 (*.txt)|*.txt"
        SaveFileDialog1.Title() = "Output Position Data"
        SaveFileDialog1.FileName() = "Position Data"
       
        SaveFileDialog1.InitialDirectory() = Application.StartupPath.Remove(Application.StartupPath.Length - 9).Insert(Application.StartupPath.Length - 9, "SaveData")
        SaveFileDialog1.ShowDialog()

        
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        Dim FileNum As Integer
        Dim strTemp As String
        
        FileNum = FreeFile()
        FileOpen(FileNum, SaveFileDialog1.FileName, OpenMode.Output)
        strTemp = "阻尼器上方質量位置響應"
        PrintLine(FileNum, strTemp)
        For i = 0 To outputData.GetLength(1) - 1
            strTemp = outputData(0, i)
            PrintLine(FileNum, strTemp)
        Next
       
        FileClose(FileNum)
    End Sub

   
 
    Private Sub PictureBox1_Click(sender As System.Object, e As System.EventArgs) Handles PictureBox1.Click

    End Sub
End Class