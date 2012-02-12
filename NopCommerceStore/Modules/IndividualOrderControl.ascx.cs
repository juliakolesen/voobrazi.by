using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.Web;
using NopSolutions.NopCommerce.BusinessLogic.Messages;

namespace  NopSolutions.NopCommerce.Web.Modules
{
    public partial class IndividualOrderControl : BaseNopUserControl
    {
        String[] users = new String[]{"Для себя","Любимой", "Молодой девушке", "Женщине", "Маме", "Бабушке", 
                                       "VIP(очень важная персона)", "Мужчине", "Начальнику"};

        String[] reasons = new String[]{"Просто так", "День рождения", "Юбилей", "Любовь", "Извинения",
                                        "Благодарность", "Соболезнования", "Банкет", "выставка", 
                                        "конференция/важное мероприятие", "встреча гостей/представителей",
                                        "Свадьба", "Новый год и Рождество", "14 февраля День Всех Влюбленных",
                                        "8 Марта", "9 мая", "Последний звонок/экзамены", 
                                        "Выпускной/встреча с выпускниками", "Первый звонок", "День Учителя", "День матери"};

        String[] flowers = new String[] { "Роза", "Хризантема", "Тюльпаны", "Альстромерия", "Амариллис",
                                          "Антуриум", "Бамбук", "Гвоздика", "Гербера", "Ирис", "Калла", "Лилия",
                                          "Орхидея", "Фриезия", "Эустома"};

        String[] colours = new String[] { "Бело-голубая", "Бело-зеленая", "Бордово-оранжевая", "Бело-розовая",
                                          "Бело-сиреневая", "Желто-красная", "Желто-оранжевая", "Желто-фиолетовая",
                                          "Красно-белая", "Красно-бордовая", "Кремовая", "Розово-сиреневая", 
                                          "Смешанная", "Синяя-Черная"};

        String[] bunches = new String[] { "Букет круглый", "Букет вертикальный", "Корзина с цветами", 
                                       "Настольная композиция из цветов", "Сердце из цветов", 
                                       "Сундук из цветов", "Игрушка из цветов", "Дерево из цветов",
                                       "Корабль из цветов", "Торт из цветов", "Коктейль из цветов"};

        String messageTemplate = "<p>Индивидуальный заказ: <br/> 1. Для кого цветы: %user% <br/> 2.	Какой повод/праздник: %reason%<br/>" +
                         "3.Какие цветы предпочитаете: %flower% <br/> 4.Какая цветовая гамма: %colour% <br/>" +
                         "5.Какой вариант букета/композиции предпочитаете: %bunch% <br/> 6. Цена: %price% <br/><br/>" +
                         "Имя: %name% <br/> Телефон: %phone%</p>";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillVariants(this.UserVariants, users);
                FillVariants(this.reasonVar, reasons);
                FillVariants(this.flowerVar, flowers);
                FillVariants(this.colourVar, colours);
                FillVariants(this.bunchVar, bunches);
            }
            
        }

        private void ChangeVisible(int index, DropDownList dropDownList, TextBox txtBox)
        {
            if (index == 1)
                dropDownList.Style["visibility"] = "visible";
            if(index == 2)
                txtBox.Style["visibility"] = "visible";
        }

        protected void fastOrder_Click(object sender, EventArgs e)
        {
            String message = GetMessage();
            MessageManager.SendQuickOrderMessage(message);
            Session.Add("quickOrderMessage", message);
            Response.Redirect("~/IndividualOrder.aspx?sentMessage=1");
        }

        private string GetMessage()
        {
            String message = messageTemplate;
            message = ReplaceParameter(message, "%user%", this.userVarList, this.UserVariants, this.ownUser);
            message = ReplaceParameter(message, "%reason%", this.reasonVarList, this.reasonVar, this.ownReason);
            message = ReplaceParameter(message, "%flower%", this.flowerVarList, this.flowerVar, this.ownFlower);
            message = ReplaceParameter(message, "%colour%", this.colourVarList, this.colourVar, this.ownColour);
            message = ReplaceParameter(message, "%bunch%", this.bunchVarList, this.bunchVar, this.ownVariant);
            message = message.Replace("%price%", this.Price.Text);
            message = message.Replace("%name%", this.nameTextBox.Text);
            message = message.Replace("%phone%", this.phoneTextBox.Text);
            return message;
        }

        private void FillVariants(DropDownList drList, String[] variants)
        {
            foreach (String variant in variants)
            {
                drList.Items.Add(variant);
            }
        }

        private string ReplaceParameter(string message, string parameterName, RadioButtonList radioButtonList,
                                        DropDownList dropDownList, TextBox textBox)
        {
            String newParameterName = String.Empty; ;
            switch (radioButtonList.SelectedIndex)
            {
                case 0:
                    newParameterName = radioButtonList.SelectedValue;
                    break;
                case 1:
                    newParameterName = dropDownList.SelectedValue;
                    break;
                case 2:
                    newParameterName = textBox.Text;
                    break;
            }

            return message.Replace(parameterName, newParameterName);
        }
    }
}