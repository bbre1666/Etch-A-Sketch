Option Strict On
Option Explicit On
Imports System.Runtime.Intrinsics.X86

'Baden Brenner
'RCET0265
'Spring 2023
'Etch-A-Sketch
'


Public Class EtchASketch
    'Globals
    Dim MainPen As New Pen(Color.Black)
    Dim GradiculeState As Boolean = False
    Dim WaveFormState As Boolean = False
    Dim scopelines As Bitmap
    Dim Waves As Bitmap
    Dim Blank As Bitmap
    Dim Wavesscopelines As Bitmap
    Dim currentColor As Color
    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click
        'Clear button
        Clear() 'Runs Clear Sub
    End Sub
    Sub Clear()
        PictureBox.BackColor = Color.White 'Set backround of pictureBox
        PictureBox.Refresh() 'Refresh pictureBox 
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        currentColor = Color.Black 'setting pen color
    End Sub


    Private Sub PictureBox_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox.MouseMove
        Static lastX%, lastY%
        Select Case e.Button.ToString 'Trackes mouse movement and button clicks 
            Case "Left"
                DrawLineSegment(lastX, lastY, e.X, e.Y)
            Case "Middle"
                UpdateColor()
            Case "Right"
                ContextMenuStrip.Show()
        End Select
        Me.Text = $"({e.X},{e.Y}) button: {e.Button.ToString} Color: {Me.currentColor.ToString}"
        lastX = e.X
        lastY = e.Y
    End Sub
    Sub DrawLineSegment(x1%, y1%, x2%, y2%) 'drawing the line
        Dim g As Graphics = PictureBox.CreateGraphics
        Dim myPen = New Pen(Me.currentColor)

        g.DrawLine(myPen, x1, y1, x2, y2)

        myPen.Dispose()
        g.Dispose()
    End Sub
    Sub GraphWaveform() 'creating the bitmap fot the graphed waveforms 
        Dim picBoxWidth As Integer = PictureBox.Width ' Define variables for the picture box dimensions
        Dim picBoxHeight As Integer = PictureBox.Height


        ' Create a bitmap to draw the sine wave on
        Dim Waveforms As New Bitmap(picBoxWidth, picBoxHeight) 'setting the bitmap size 

        ' Set up the graphics object
        Dim g As Graphics = Graphics.FromImage(Waveforms)

        ' Define variables for the sine wave
        Dim amplitude As Double = picBoxHeight / 4 ' the amplitude of the wave
        Dim frequency As Double = 2 * Math.PI / picBoxWidth ' the frequency of the wave

        ' Draw the sine wave
        For x As Integer = 0 To picBoxWidth - 1
            Dim y As Integer = CInt(amplitude * Math.Sin(frequency * x))
            Waveforms.SetPixel(x, picBoxHeight \ 2 - y, Color.Black)
        Next
        ' Draw the Cosine wave
        For x As Integer = 0 To picBoxWidth - 1
            Dim y As Integer = CInt(amplitude * Math.Cos(frequency * x))
            Waveforms.SetPixel(x, picBoxHeight \ 2 - y, Color.Red)
        Next
        ' Draw the tan curve 
        Try
            For x As Integer = 0 To picBoxWidth - 1
                Dim y As Integer = CInt(amplitude * Math.Tan(frequency * x))
                Waveforms.SetPixel(x, picBoxHeight \ 2 - y, Color.Green)
            Next
        Catch
        End Try
        ' Draw the bitmap on the picture box
        Waves = Waveforms
    End Sub

    Sub UpdateColor() 'sub for updating the color of the pen
        ColorDialog.ShowDialog()
        Me.currentColor = ColorDialog.Color
    End Sub
    Private Sub SelectColorButton_Click(sender As Object, e As EventArgs) Handles SelectColorButton.Click
        UpdateColor() 'refferenceing the update color sub
    End Sub

    Private Sub ExitButton_Click(sender As Object, e As EventArgs) Handles ExitButton.Click
        Me.Close() 'closes the form
    End Sub
    Sub scopegradicules() 'creating the bitmap with to plot the gradicules  
        Dim picBoxWidth As Integer = PictureBox.Width ' Define variables for the picture box dimensions
        Dim picBoxHeight As Integer = PictureBox.Height
        Dim GridSize As Integer = CInt(77.5) 'setting the size of the grid

        Dim Gradicules As New Bitmap(picBoxWidth, picBoxHeight) 'setting the bitmap size 

        Dim G As Graphics = Graphics.FromImage(Gradicules)
        ' Draw the horizontal lines
        For y As Integer = 0 To picBoxHeight - 1 Step GridSize
            G.DrawLine(Pens.Black, New Point(0, y), New Point(picBoxWidth, y))
        Next

        ' Draw the vertical lines
        For x As Integer = 0 To picBoxWidth - 1 Step GridSize
            G.DrawLine(Pens.Black, New Point(x, 0), New Point(x, picBoxHeight))
        Next

        ' Draw the bitmap on the picture box

        scopelines = Gradicules
    End Sub
    Private Sub WaveformsButton_Click(sender As Object, e As EventArgs) Handles WaveformsButton.Click

        If WaveFormState = False Then 'checking the wavefrom latch and the state of it 
            WaveFormState = True
        ElseIf WaveFormState = True Then
            WaveFormState = False
        End If

        If WaveFormState = True Then ' displaying or clearing the waveform
            GraphWaveform()
            PictureBox.Image = Waves

        ElseIf WaveFormState = False Then
            Clear()
            PictureBox.Image = Blank
        End If
        BothMaps() 'checking for both gradicules and waveforms

    End Sub

    Private Sub gradicules_Click(sender As Object, e As EventArgs) Handles GradiculesButton.Click

        If GradiculeState = False Then  'checking the Gradicules latch and the state of it 
            GradiculeState = True
        ElseIf GradiculeState = True Then
            GradiculeState = False
        End If

        If GradiculeState = True Then ' displaying or clearing the Gradicules
            scopegradicules()
            PictureBox.Image = scopelines

        ElseIf GradiculeState = False Then
            Clear()
            PictureBox.Image = Blank
        End If
        BothMaps() 'checking for both gradicules and waveforms
    End Sub
    Private Sub BothMaps()
        'this sub is checked when ether the waveform and Gradicule button is clicked to see if they need to be displayed together 

        If GradiculeState And WaveFormState Then
            Dim picBoxWidth As Integer = PictureBox.Width ' Define variables for the picture box dimensions
            Dim picBoxHeight As Integer = PictureBox.Height

            ' Create a new bitmap to hold the combined images adn setting the size 
            Dim combinedBmp As New Bitmap(picBoxWidth, picBoxHeight)

            Dim g As Graphics = Graphics.FromImage(combinedBmp)


            g.DrawImage(Waves, New Point(0, 0))


            g.DrawImage(scopelines, New Point(10, 10))


            g.Dispose()

            ' Save the combined image to a file or display it in a picture box
            combinedBmp.Save("combined.bmp")

            PictureBox.Image = combinedBmp 'displaying the combined bitmap
        End If
    End Sub
End Class
