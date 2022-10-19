Imports System.Threading


Public Class Form1
    ' Use a dictionary instead of a list
    ' dictionaries are key-value pairs which help in identifying unique sets of data
    ' keys are used to get the value
    ReadOnly countries As New Dictionary(Of String, String) From
    {
        {"Philippines", "Manila"},
        {"Austria", "Vienna"},
        {"Switzerland", "Bern"}
    }
    ' Timeout is used as a callback which is called after running the pause
    ' The callback is a method/function which handles what logic should be used
    ' after the timeout succeeds
    ' in this case, we use the intializeCountryInvoke as the logic that handles
    ' what happens after the timeout (e.g., after 3 seconds)
    ReadOnly onTimeout As TimerCallback = AddressOf initializeCountryInvoke
    ' generator is a random integer generator
    ReadOnly generator As System.Random = New System.Random()

    ' currentCountry holds the current value for checking the current state
    Dim currentCountry As KeyValuePair(Of String, String) = Nothing

    ' this is a constructor which initializes or "bootstraps" or "starts" (if you will) the class
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        pickCountry()
        lblCountry.Text = currentCountry.Key
    End Sub


    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click

        Dim message As String

        ' Checks if currentCountry's capital (the value) is equal to the one written
        If txtCapital.Text.ToLower() = currentCountry.Value.ToLower() Then
            message = "Correct!"
        Else
            message = "Wrong " + currentCountry.Value
        End If

        lblAnswer.Text = message

        ' the timer is run, and after 3seconds would run onTimeout property above
        Dim t As New Timer(onTimeout, Nothing, 3000, -1)

    End Sub

    ' this just picks a random country
    Private Sub pickCountry()
        ' gets the total number of elements
        Dim count As UInt16 = countries.Count

        ' gets a random key-value pair
        Dim kv = countries.ElementAt(generator.Next(0, count))
        ' and then deletes it from the list
        countries.Remove(kv.Key)

        ' sets the key-value pair to the currentCountry so that it's visible
        ' to the whole class, and we can use for identifying correct answer later
        currentCountry = kv
    End Sub


    Private Sub initializeCountryInvoke()
        ' pick another country after running the timeout
        pickCountry()

        ' Joins threads, error pops out because callback functions are handled by a different thread
        lblCountry.Invoke(New MethodInvoker(Sub() lblCountry.Text = currentCountry.Key))
        lblAnswer.Invoke(New MethodInvoker(Sub() lblAnswer.Text = ""))
        txtCapital.Invoke(New MethodInvoker(Sub() txtCapital.Clear()))
    End Sub

End Class