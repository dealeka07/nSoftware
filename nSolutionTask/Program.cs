using System;
using System.Collections.Generic;
using nsoftware.IPWorksSSL;
using nsoftware.IPWorksMQ;

class getAndParse
{
    private static HTTP http1 = new nsoftware.IPWorksSSL.HTTP();

    private static JSON json = new nsoftware.IPWorksMQ.JSON();

    private static void http1_OnSSLServerAuthentication(object sender, nsoftware.IPWorksSSL.HTTPSSLServerAuthenticationEventArgs e)
    {
        if (e.Accept) return;
        Console.Write("Server provided the following certificate:\nIssuer: " + e.CertIssuer + "\nSubject: " + e.CertSubject + "\n");
        Console.Write("The following problems have been determined for this certificate: " + e.Status + "\n");
        Console.Write("Would you like to continue anyways? [y/n] ");
        if (Console.Read() == 'y') e.Accept = true;
    }

    private static void http1_OnTransfer(object sender, HTTPTransferEventArgs e)
    {
        Console.WriteLine("Response from server:");
        Console.WriteLine(e.Text);

        try
        {
            json.BuildDOM = true;
            json.InputData = e.Text;
            json.Parse();
            string jsonPath = "/json/products";
            
            json.XPath = jsonPath;
            int productCount = json.XChildren.Count;

            Console.WriteLine("Products Title");
            for (int i = 1; i <= productCount; i++)
            {
                Console.WriteLine("\nBook #" + i);

                json.XPath = "/json/products/[" + i + "]/title";  // XPath to select title for current product
                string title = json.XText;  // Get the title text once

                Console.WriteLine("Title: " + title);  // Print the title once
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static void Main()
    {
            string url = "https://dummyjson.com/products";
            http1.OnTransfer += http1_OnTransfer;
            http1.OnSSLServerAuthentication += http1_OnSSLServerAuthentication;


                    try
                    {
                        http1.Get(url);
                        Console.WriteLine("Press enter to continue...");
                        Console.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }


        Console.WriteLine("\npress <return> to continue...");
        Console.Read();
        
    }

}