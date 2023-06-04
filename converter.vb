Imports System.Net
Imports System.IO
Imports Newtonsoft.Json.Linq

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim currencies As String() = {"USD", "EUR", "GBP", "JPY", "CAD", "AUD"} 
        cmbFromCurrency.Items.AddRange(currencies)
        cmbToCurrency.Items.AddRange(currencies)

        cmbFromCurrency.SelectedIndex = 0
        cmbToCurrency.SelectedIndex = 1
    End Sub

    Private Sub btnConvert_Click(sender As Object, e As EventArgs) Handles btnConvert.Click
        Dim fromCurrency As String = cmbFromCurrency.SelectedItem.ToString()
        Dim toCurrency As String = cmbToCurrency.SelectedItem.ToString()
        Dim amount As Double = CDbl(txtAmount.Text)

        Dim apiKey As String = "YOUR_API_KEY" ' Replace with your own API key
        Dim apiUrl As String = $"https://v6.exchangeratesapi.io/latest?base={fromCurrency}&symbols={toCurrency}&access_key={apiKey}"
        Dim exchangeRate As Double

        Try
            Dim request As HttpWebRequest = WebRequest.Create(apiUrl)
            Dim response As HttpWebResponse = request.GetResponse()
            Dim dataStream As Stream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()

            Dim json As JObject = JObject.Parse(responseFromServer)
            exchangeRate = CDbl(json.SelectToken($"rates.{toCurrency}"))
        Catch ex As Exception
            MessageBox.Show("Error retrieving exchange rate. Please try again later.")
            Return
        End Try

        Dim convertedAmount As Double = amount * exchangeRate

        lblResult.Text = $"{amount} {fromCurrency} = {convertedAmount} {toCurrency}"
    End Sub
End Class
