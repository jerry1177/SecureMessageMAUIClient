namespace SecureMessageMAUIClient;

using System.Security.Cryptography;
using System.Text;

public partial class XamlPlusCodePage : ContentPage
{
	public XamlPlusCodePage()
	{
		InitializeComponent();
	}

    void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
    {
        valueLabel.Text = args.NewValue.ToString("F3");
    }

    async void OnButtonClicked(object sender, EventArgs args)
    {
        Button button = (Button)sender;
        string plainText = "this is the privateKey";

        RSA rsa1 = RSA.Create(4096);
        RSAParameters publicParams1 = rsa1.ExportParameters(false);

        

        RSA rsa2 = RSA.Create(4096);
        RSAParameters publicParams2 = rsa2.ExportParameters(false);

        RSA publicRSA1 = RSA.Create();
        publicRSA1.ImportParameters(publicParams1);

        RSA publicRSA2 = RSA.Create();
        publicRSA2.ImportParameters(publicParams2);

        byte[] signature = rsa1.SignData(Encoding.ASCII.GetBytes(plainText), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        //byte[] encryptedText = publicRSA2.Encrypt(Encoding.ASCII.GetBytes(plainText), RSAEncryptionPadding.OaepSHA256);
        string message = Encoding.ASCII.GetString(signature) + ":" + plainText;
        byte[] encryptedMessage = publicRSA2.Encrypt(Encoding.ASCII.GetBytes(message), RSAEncryptionPadding.OaepSHA256);
        //byte[] encryptedText = publicRSA.Encrypt(Encoding.ASCII.GetBytes(plainText), RSAEncryptionPadding.OaepSHA256);


        //string message = Encoding.ASCII.GetString(signature) + ":" + plainText;
        //byte[] encryptedText = publicRSA1.Encrypt(Encoding.ASCII.GetBytes(message), RSAEncryptionPadding.OaepSHA256);


        //receiver
        byte[] decriptedMessage = rsa2.Decrypt(encryptedMessage, RSAEncryptionPadding.OaepSHA256);

        string[] messageParts = Encoding.ASCII.GetString(decriptedMessage);

        if (publicRSA1.VerifyData(Encoding.ASCII.GetBytes(messageParts[1]), Encoding.ASCII.GetBytes(messageParts[0]), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1)) {
            await DisplayAlert("Clicked!", "it worked", "OK");
        }

        byte[] encryptedText2 = rsa1.Encrypt(Encoding.ASCII.GetBytes(plainText), RSAEncryptionPadding.OaepSHA256);

        //byte[] plaintext2 = rsa1.Decrypt(encryptedText, RSAEncryptionPadding.OaepSHA256);
        byte[] plaintext3 = rsa1.Decrypt(encryptedText2, RSAEncryptionPadding.OaepSHA256);

        string privateKey = Convert.ToBase64String(rsa1.ExportRSAPrivateKey());

        //RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        await DisplayAlert("Clicked!", "private key: " + privateKey, "OK");

        //await DisplayAlert("Clicked!", "text1: " + Encoding.ASCII.GetString(plaintext2) + "\nEncrypted text 2: " + Encoding.ASCII.GetString(plaintext3), "OK");
    }
}