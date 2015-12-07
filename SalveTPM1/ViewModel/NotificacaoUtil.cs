using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace SalveTPM1.ViewModel
{
    class NotificacaoUtil
    {

        public void showNotificacao(String texto, String caminhoImagem, String parametro)
        {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText02;

            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(texto));

            XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
            ((XmlElement)toastImageAttributes[0]).SetAttribute("src", caminhoImagem);
            ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "Notificação Salve TPM");

            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("duration", "long");

            XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.IM");
            audio.SetAttribute("loop", "true"); // somente para long, notificacoes longas.

            toastNode.AppendChild(audio);


            //passando parametros para a notificacao
            ((XmlElement)toastNode).SetAttribute("launch", "NOTIFICACAO:" + parametro);

            ToastNotification toast = new ToastNotification(toastXml);

            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }


        public void agendarNotificacao(String texto, String caminhoImagem, String parametro, DateTime dataNotificacao)
        {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText02;

            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(texto));

            XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
            ((XmlElement)toastImageAttributes[0]).SetAttribute("src", caminhoImagem);
            ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "Notificação Salve TPM");

            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("duration", "long");

            XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.IM");
            audio.SetAttribute("loop", "true"); // somente para long, notificacoes longas.

            toastNode.AppendChild(audio);


            //passando parametros para a notificacao
            ((XmlElement)toastNode).SetAttribute("launch", "{\"type\":\"toast\",\"NOTIFICACAO\":\"" + parametro + "\"}");

           

            ScheduledToastNotification scheduledToast = new ScheduledToastNotification(toastXml, dataNotificacao);
            //scheduledToast.Id = "SALVETPM";

            ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduledToast);
        }



    }
}
