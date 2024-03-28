Imports System.Data.OleDb

Public Class Form1
    Public con As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\casa\Desktop\Base de datos\Registro.mdb")
    Public cmd As New OleDbCommand
    Public sql As String
    Public texto As String = "Escribe en los comentarios, que otros proyectos deseas y dale LIKE si te gusto el vídeo   "
    Dim textosub As String = ""
    Dim c As Integer




    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
            con.Open()
            MessageBox.Show("Se conectó correctamente con la base de datos")
            ControlesIniciales()
            MostrarRegistros()
            Timer1.Start()
        Catch ex As Exception
            MessageBox.Show("Base de datos no encontrada " + ex.Message)
        End Try
    End Sub
    Public Sub limpiarcajas()
        For Each caja In gbdatos.Controls
            If TypeOf (caja) Is TextBox Then
                caja.Text = ""
                caja.Enabled = True
            End If
        Next
    End Sub
    Public Sub ControlesIniciales()
        btnnuevo.Visible = True
        btnagregar.Visible = False

        btnmodificar.Visible = True
        btnaceptar.Visible = False

        btneliminar.Visible = True
        btncancelar.Visible = False

        btnbuscar.Visible = False
        picfoto.Image = Nothing
        limpiarcajas()
        gbdatos.Enabled = False
    End Sub
    Public Sub MostrarRegistros()
        sql = ""
        sql = "SELECT id_persona, nombre, apellido, telefono, foto FROM personas"
        cmd = New OleDbCommand(sql, con)
        Dim adp As New OleDbDataAdapter(cmd)
        Dim dt As New DataTable
        adp.Fill(dt)
        dgregistros.DataSource = dt

        Dim c As Integer
        c = dt.Rows.Count
        gbregistros.Text = "Registros Totales " + c.ToString()
        dgregistros.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill 'rellena todo el data gridview
        dgregistros.Columns.Item(4).Visible = False
        dgregistros.AllowUserToAddRows = False
    End Sub

    Private Sub btnnuevo_Click(sender As Object, e As EventArgs) Handles btnnuevo.Click
        limpiarcajas()
        btnnuevo.Visible = False
        btnagregar.Visible = True

        btnmodificar.Visible = False
        btnaceptar.Visible = False

        btncancelar.Visible = True
        btneliminar.Visible = False

        btnbuscar.Visible = True
        picfoto.Image = Nothing
        gbdatos.Enabled = True
    End Sub

    Private Sub btncancelar_Click(sender As Object, e As EventArgs) Handles btncancelar.Click
        ControlesIniciales()
    End Sub

    Private Sub btnmodificar_Click(sender As Object, e As EventArgs) Handles btnmodificar.Click
        Dim condicion As Boolean = txtcodigo.Text <> "" And txtnombre.Text <> "" And txtapellido.Text <> "" And txttelefono.Text <> ""

        If condicion Then
            txtcodigo.Enabled = False
            btnmodificar.Visible = False
            btnaceptar.Visible = True

            btnnuevo.Visible = False
            btnagregar.Visible = False

            btncancelar.Visible = True
            btneliminar.Visible = False

            btnbuscar.Visible = True

            gbdatos.Enabled = True
        Else
            MessageBox.Show("Seleccione un registro", "Registro informa", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If


    End Sub
    Private Sub btneliminar_Click(sender As Object, e As EventArgs) Handles btneliminar.Click
        Dim condicion As Boolean = txtcodigo.Text <> "" And txtnombre.Text <> "" And txtapellido.Text <> "" And txttelefono.Text <> ""


        If condicion Then
            Dim pregunta As DialogResult = MessageBox.Show("Desea eliminar el registro seleccionado", "Registro Informa", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
            If pregunta = Windows.Forms.DialogResult.OK Then
                sql = ""
                sql = "DELETE FROM personas WHERE id_persona = @cod"
                cmd = New OleDbCommand(sql, con)
                cmd.Parameters.Add(New OleDbParameter("@cod", txtcodigo.Text))
                If cmd.ExecuteNonQuery Then
                    MessageBox.Show("Registro elminado")
                    MostrarRegistros()
                    ControlesIniciales()
                Else
                    MessageBox.Show("Registro no eliminado")
                End If
            End If
           
        Else
            MessageBox.Show("Seleccione un registro", "Registro informa", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    Private Sub btnagregar_Click(sender As Object, e As EventArgs) Handles btnagregar.Click
        Dim condicion As Boolean = txtcodigo.Text <> "" And txtnombre.Text <> "" And txtapellido.Text <> "" And txttelefono.Text <> ""
        If condicion Then
            Try
                sql = ""
                sql = "INSERT INTO personas VALUES(@cod,@nom,@Ape,@tel,@fot)"
                cmd = New OleDbCommand(sql, con)
                cmd.Parameters.Add(New OleDbParameter("@cod", txtcodigo.Text))
                cmd.Parameters.Add(New OleDbParameter("@nom", txtnombre.Text))
                cmd.Parameters.Add(New OleDbParameter("@ape", txtapellido.Text))
                cmd.Parameters.Add(New OleDbParameter("@tel", txttelefono.Text))

                Dim i() As Byte
                Dim ms As New IO.MemoryStream()

                If picfoto.Image Is Nothing Then
                    picfoto.Image = My.Resources.Sin_imagen_disponible
                End If
                picfoto.SizeMode = PictureBoxSizeMode.StretchImage
                picfoto.Image.Save(ms, picfoto.Image.RawFormat)
                i = ms.GetBuffer

                cmd.Parameters.Add(New OleDbParameter("@fot", i))

                If cmd.ExecuteNonQuery Then
                    MessageBox.Show("Registro correcto", "Registro Informa", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ControlesIniciales()
                    MostrarRegistros()
                Else
                    MessageBox.Show("Registro no correcto", "Registro Informa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    ControlesIniciales()
                    MostrarRegistros()
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        Else
            MessageBox.Show("Rellene todos los Datos", "Registro Informa", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        
    End Sub

    Private Sub btnaceptar_Click(sender As Object, e As EventArgs) Handles btnaceptar.Click
        Dim condicion As Boolean = txtcodigo.Text <> "" And txtnombre.Text <> "" And txtapellido.Text <> "" And txttelefono.Text <> ""
        If condicion Then
            Try
                sql = ""
                sql = "UPDATE personas SET id_persona = @cod, nombre = @nom, apellido = @ape, telefono = @tel, foto = @fot WHERE id_persona = @cod "
                cmd = New OleDbCommand(sql, con)
                cmd.Parameters.Add(New OleDbParameter("@cod", txtcodigo.Text))
                cmd.Parameters.Add(New OleDbParameter("@nom", txtnombre.Text))
                cmd.Parameters.Add(New OleDbParameter("@ape", txtapellido.Text))
                cmd.Parameters.Add(New OleDbParameter("@tel", txttelefono.Text))

                Dim i() As Byte
                Dim ms As New IO.MemoryStream()

                If picfoto.Image Is Nothing Then
                    picfoto.Image = My.Resources.Sin_imagen_disponible
                End If
                picfoto.SizeMode = PictureBoxSizeMode.StretchImage
                picfoto.Image.Save(ms, picfoto.Image.RawFormat)
                i = ms.GetBuffer

                cmd.Parameters.Add(New OleDbParameter("@fot", i))

                If cmd.ExecuteNonQuery Then
                    MessageBox.Show("Modificado correcto", "Registro Informa", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ControlesIniciales()
                    MostrarRegistros()
                Else
                    MessageBox.Show("Modificado no correcto", "Registro Informa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    ControlesIniciales()
                    MostrarRegistros()
                End If
            

            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        Else
            MessageBox.Show("Rellene todos los Datos", "Registro Informa", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub dgregistros_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgregistros.CellClick
        txtcodigo.Text = dgregistros.SelectedCells.Item(0).Value
        txtnombre.Text = dgregistros.SelectedCells.Item(1).Value
        txtapellido.Text = dgregistros.SelectedCells.Item(2).Value
        txttelefono.Text = dgregistros.SelectedCells.Item(3).Value

        picfoto.Image = Nothing

        Dim i() As Byte = dgregistros.SelectedCells.Item(4).Value
        Dim ms As New IO.MemoryStream(i)

        picfoto.Image = Image.FromStream(ms)
        picfoto.SizeMode = PictureBoxSizeMode.StretchImage
    End Sub

    Private Sub btnbuscar_Click(sender As Object, e As EventArgs) Handles btnbuscar.Click
        Dim openfiledialog As New OpenFileDialog

        openfiledialog.Filter = "Imagen jpg|*.jpg"
        If openfiledialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            'picfoto.Image = Image.FromFile(openfiledialog.FileName)
            picfoto.Image = New Bitmap(openfiledialog.FileName)
            picfoto.SizeMode = PictureBoxSizeMode.StretchImage
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
       
        Dim largo As Integer = texto.Length - 1
        textosub += texto.Substring(c, 1)
        c += 1
        lbltexto.Text = textosub

        If largo = c Then
            c = 0
            textosub = ""
        End If
    End Sub
End Class
