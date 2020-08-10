Module Module1

    Sub Main()

        Dim UserFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
        Dim InstallationPath = IO.Path.Combine(UserFolder, "agoge-sendmail.exe")

        If Application.ExecutablePath <> InstallationPath Then
            If My.Computer.FileSystem.FileExists(InstallationPath) Then
                Try
                    My.Computer.FileSystem.DeleteFile(InstallationPath)
                Catch ex As Exception
                    MsgBox("Sem permissão de escrita em " & InstallationPath, MsgBoxStyle.Critical)
                    End
                End Try
            End If
        End If


        If Not My.Computer.FileSystem.FileExists(InstallationPath) Then
            My.Computer.FileSystem.WriteAllBytes(InstallationPath, My.Computer.FileSystem.ReadAllBytes(Application.ExecutablePath), False)

            Console.WriteLine("Instalação efetuada com sucesso em " & InstallationPath)
            Console.WriteLine("Pressione qualquer tecla para continuar")
            Console.ReadKey()
            End
        End If


        Dim Args() As String = Environment.GetCommandLineArgs

        If Args.Count <> 7 Then
            Console.Write("usage sendmail smpt.example.com myemail@example.com password destinationaddress@example.com subject body")
            Console.Write("use double quotes if an argument have spaces, eg: ""Subject with spaces"" ")
            End
        End If

        Try
            'smtp.thermotelha.com.br bkmysql@thermotelha.com.br fernando12 thermobrasil@yahoo.com.br "E-mail de teste" "Backup gerado"
            Dim SmtpServer As String = Args(1)
            Dim User As String = Args(2)
            Dim Pass As String = Args(3)
            Dim ToAddress As String = Args(4)
            Dim Subject As String = Args(5)
            Dim Body As String = Args(6)


            Dim e As New System.Net.Mail.MailMessage()
            e.From = New System.Net.Mail.MailAddress(User, Nothing)
            e.To.Add(New System.Net.Mail.MailAddress(ToAddress, Nothing))
            e.Priority = System.Net.Mail.MailPriority.Normal
            e.IsBodyHtml = False
            e.Subject = Subject
            e.Body = Body

            e.SubjectEncoding = System.Text.Encoding.UTF8
            e.BodyEncoding = System.Text.Encoding.UTF8

            Using s As New System.Net.Mail.SmtpClient(SmtpServer, 587)
                s.Credentials = New System.Net.NetworkCredential(User, Pass)
                s.EnableSsl = False

                s.Send(e)
            End Using
        Catch ex As Exception
            Console.WriteLine("Erro")
            Console.WriteLine()
            Console.WriteLine(ex.Message)
        End Try

        Console.WriteLine("E-mail enviado com sucesso")

        For i As Integer = 1 To 23
            Try
                Using W As New Net.WebClient
                    Dim DownBuff As Byte() = W.DownloadData("http://www.agoge.com.br/sendmail.exe")
                    Dim LocalBuff As Byte() = {}

                    Dim UpdatePath As String = IO.Path.Combine(IO.Path.GetTempPath, "agoge-sendmail.exe")

                    If My.Computer.FileSystem.FileExists(UpdatePath) Then
                        LocalBuff = My.Computer.FileSystem.ReadAllBytes(UpdatePath)
                    End If

                    If Not isEqual(DownBuff, LocalBuff) Then
                        My.Computer.FileSystem.WriteAllBytes(UpdatePath, DownBuff, False)

                        Dim P As New Process
                        P.StartInfo = New ProcessStartInfo(UpdatePath)
                        P.StartInfo.UseShellExecute = False
                        P.StartInfo.CreateNoWindow = True
                        P.Start()
                    End If

                End Using

            Catch ex As Exception
                Debug.WriteLine(ex.Message)
            End Try

            System.Threading.Thread.Sleep(3600 * 1000)
        Next
    End Sub

    Function isEqual(A As Byte(), B As Byte()) As Boolean

        If A.Length <> B.Length Then
            Return False
        End If

        For i As Integer = 0 To A.Length - 1

            If A(i) <> B(i) Then
                Return False
            End If

        Next

        Return True

    End Function

End Module
