using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


namespace WznGwent
{
    /// <summary>
    /// ConfigWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigWindow : Window
    {
        public ConfigWindow()
        {
            InitializeComponent();

            XmlSerializer serializer = new XmlSerializer(typeof(CardsModel));
            if(File.Exists("config.xml"))
            {
                using (Stream xmlDocument = new FileStream("config.xml", FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    cardModel = (CardsModel)serializer.Deserialize(xmlDocument);
                }
            }
           

            this.leadCard.DataContext = cardModel.LeaderCard;
            this.leaderStack.DataContext = cardModel.LeaderCard;
            this.infoStack.DataContext = cardModel;
            deckCardsAll.ItemsSource = cardModel.Deckcards;
            this.leadersFlipView.ItemsSource = leaders;

            deckCardsCloseCombat.ItemsSource = cardModel.Deckcards.FindAll(x => x.Range == CardFaceRanges.CloseCombat);
            deckCardsRanged.ItemsSource = cardModel.Deckcards.FindAll(x => x.Range == CardFaceRanges.Ranged);
            deckCardsSiege.ItemsSource = cardModel.Deckcards.FindAll(x => x.Range == CardFaceRanges.Siege);
            deckCardsHero.ItemsSource = cardModel.Deckcards.FindAll(x => x.Hero);
            deckCardsWeather.ItemsSource = cardModel.Deckcards.FindAll(x => x.Ability == CardFaceAbilities.ImpenetrableFog || x.Ability == CardFaceAbilities.TorrentialRain
                    || x.Ability == CardFaceAbilities.ImpenetrableFog || x.Ability == CardFaceAbilities.BitingFrost
                    || x.Ability == CardFaceAbilities.SkelligeStorm || x.Ability == CardFaceAbilities.ClearWeather);
            deckCardsMagic.ItemsSource = cardModel.Deckcards.FindAll(x => x.Range == CardFaceRanges.Null);

            remainingCardsAll.ItemsSource = cardModel.Deckcards;
            remainingCardsCloseCombat.ItemsSource = cardModel.Deckcards.FindAll(x => x.Range == CardFaceRanges.CloseCombat);
            remainingCardsRanged.ItemsSource = cardModel.Deckcards.FindAll(x => x.Range == CardFaceRanges.Ranged);
            remainingCardsSiege.ItemsSource = cardModel.Deckcards.FindAll(x => x.Range == CardFaceRanges.Siege);
            remainingCardsHero.ItemsSource = cardModel.Deckcards.FindAll(x => x.Hero);
            remainingCardsWeather.ItemsSource = cardModel.Deckcards.FindAll(x => x.Ability == CardFaceAbilities.ImpenetrableFog || x.Ability == CardFaceAbilities.TorrentialRain
                     || x.Ability == CardFaceAbilities.ImpenetrableFog || x.Ability == CardFaceAbilities.BitingFrost
                     || x.Ability == CardFaceAbilities.SkelligeStorm || x.Ability == CardFaceAbilities.ClearWeather);
            remainingCardsMagic.ItemsSource = cardModel.Deckcards.FindAll(x => x.Range==CardFaceRanges.Null);

        }

        List<CardFace> leaders = new List<CardFace>{
                new CardFace() { Face = "Tw3_cardart_northernrealms_redania", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.CloseCombat, Power = 1,
                    Name="可怜的步兵"},
                new CardFace() { Face = "Tw3_cardart_northernrealms_poor_infantry", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.CloseCombat, Power = 1, Ability=CardFaceAbilities.TightBond,
                    Name="瑞达尼亚步军" },
                new CardFace() { Face = "Tw3_cardart_northernrealms_yarpen", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.CloseCombat, Power = 2,
                    Limit=1, Name="亚尔潘·齐格林"},
                new CardFace() { Face = "Tw3_cardart_northernrealms_blue_stripes", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.CloseCombat, Power = 4, Ability=CardFaceAbilities.TightBond,
                    Name="蓝衣铁卫突击队"}
        };

        CardsModel cardModel = new CardsModel(
            new CardFace()
            {
                Power = 1,
                Face = "Tw3_cardart_northernrealms_leader_foltest_copper",
                Amount = 1,
                Name = "攻城之王弗尔泰斯特",
                Faction = CardFaceFactions.NorthernRealms,
                Description = "将所有攻城单位的力量加倍（除非该列已经有指挥号角）"
            },
            new List<CardFace>{
                new CardFace() { Face = "Tw3_cardart_northernrealms_redania", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.CloseCombat, Power = 1,
                    Name="可怜的步兵"},
                new CardFace() { Face = "Tw3_cardart_northernrealms_poor_infantry", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.CloseCombat, Power = 1, Ability=CardFaceAbilities.TightBond,
                    Name="瑞达尼亚步军" },
                new CardFace() { Face = "Tw3_cardart_northernrealms_yarpen", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.CloseCombat, Power = 2,
                    Limit=1, Name="亚尔潘·齐格林"},
                new CardFace() { Face = "Tw3_cardart_northernrealms_blue_stripes", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.CloseCombat, Power = 4, Ability=CardFaceAbilities.TightBond,
                    Name="蓝衣铁卫突击队"},
                new CardFace() { Face = "Tw3_cardart_northernrealms_dijkstra", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.CloseCombat, Power = 4,Ability=CardFaceAbilities.Spy,
                    Limit=1, Name="西吉斯蒙德·迪杰斯特拉"},
                new CardFace() { Face = "Tw3_cardart_northernrealms_stennis", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.CloseCombat, Power = 5,Ability = CardFaceAbilities.Spy,
                    Limit=1, Name="斯坦尼斯王子"},
                new CardFace() { Face = "Tw3_cardart_northernrealms_ves", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.CloseCombat, Power = 5,
                    Limit=1, Name="薇丝"},
                new CardFace() { Face = "Tw3_cardart_northernrealms_siegfried", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.CloseCombat, Power = 5,
                    Limit=1, Name="德内斯勒的齐格弗里德"},
                new CardFace() { Face = "Tw3_cardart_northernrealms_esterad", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.CloseCombat, Power = 10, 
                    Limit=1, Name="伊斯特拉德·蒂森",Hero=true},
                new CardFace() { Face = "Tw3_cardart_northernrealms_natalis", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.CloseCombat, Power = 10, 
                    Limit=1, Name="约翰·纳塔利斯",Hero=true},
                new CardFace() { Face = "Tw3_cardart_northernrealms_vernon", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.CloseCombat, Power = 10, 
                    Limit=1,Name="弗农·罗契",Hero=true},
                 new CardFace() { Face = "Tw3_cardart_northernrealms_philippa", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.Ranged, Power = 10,
                    Limit=1,Name="菲丽芭·艾哈特",Hero=true},
                  new CardFace() { Face = "Tw3_cardart_northernrealms_sabrina", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.Ranged, Power = 4,
                    Limit=1,Name="萨宾娜·葛丽维希格"},
                  new CardFace() { Face = "Tw3_cardart_northernrealms_sheldon", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.Ranged, Power = 4,
                    Limit=1,Name="谢尔顿·斯卡格斯"},
                  new CardFace() { Face = "Tw3_cardart_northernrealms_crinfrid", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.Ranged, Power = 5, Ability = CardFaceAbilities.TightBond,
                    Name="克林菲德掠夺者巨龙猎手"},
                  new CardFace() { Face = "Tw3_cardart_northernrealms_keira", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.Ranged, Power = 5,
                    Limit=1,Name="凯拉·梅兹"},
                  new CardFace() { Face = "Tw3_cardart_northernrealms_sheala", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.Ranged, Power = 5,
                    Limit=1,Name="席儿·德·坦沙维耶"},
                  new CardFace() { Face = "Tw3_cardart_northernrealms_dethmold", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.Ranged, Power = 6,
                    Limit=1,Name="戴斯摩"},
                  new CardFace() { Face = "Tw3_cardart_northernrealms_thaler", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.Siege, Power = 1, Ability = CardFaceAbilities.Spy,
                    Limit=1,Name="塔勒"},
                  new CardFace() { Face = "Tw3_cardart_northernrealms_kaedwen_siege", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.Siege, Power = 1, Ability = CardFaceAbilities.MoraleBoost,
                    Name="科德温攻城专家"},
                  new CardFace() { Face = "Tw3_cardart_northernrealms_banner_nurse", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.Siege, Power = 5, Ability = CardFaceAbilities.Medic,
                    Limit=1,Name="褐旗营医生"},
                  new CardFace() { Face = "Tw3_cardart_northernrealms_siege_tower", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.Siege, Power = 6,
                    Limit=1,Name="攻城塔"},
                  new CardFace() { Face = "Tw3_cardart_northernrealms_ballista", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.Siege, Power = 6,
                    Limit=2,Name="弩炮"},
                  new CardFace() { Face = "Tw3_cardart_northernrealms_trebuchet", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.Siege, Power = 6,
                    Limit=2,Name="投石机"},
                  new CardFace() { Face = "Tw3_cardart_northernrealms_catapult_1", Faction=CardFaceFactions.NorthernRealms, Range = CardFaceRanges.Siege, Power = 8, Ability = CardFaceAbilities.TightBond,
                    Limit=2,Name="抛石机"},

                new CardFace() { Face = "Tw3_cardart_special_frost", Faction=CardFaceFactions.Neutral, Range = CardFaceRanges.Null, Ability = CardFaceAbilities.BitingFrost,
                    Name="刺骨冰霜"},
                new CardFace() { Face = "Tw3_cardart_special_clearsky", Faction=CardFaceFactions.Neutral, Range = CardFaceRanges.Null, Ability = CardFaceAbilities.ClearWeather,
                    Name="晴天"},
                new CardFace() { Face = "Tw3_cardart_special_fog", Faction=CardFaceFactions.Neutral, Range = CardFaceRanges.Null, Ability = CardFaceAbilities.ImpenetrableFog,
                    Name="蔽日浓雾"},
                new CardFace() { Face = "Tw3_cardart_special_rain", Faction=CardFaceFactions.Neutral, Range = CardFaceRanges.Null, Ability = CardFaceAbilities.TorrentialRain,
                    Name="倾盆大雨"},
                new CardFace() { Face = "Tw3_cardart_skellige_special_skellige_storm", Faction=CardFaceFactions.Neutral, Range = CardFaceRanges.Null, Ability = CardFaceAbilities.SkelligeStorm,
                    Name="史凯利杰风暴"},
                new CardFace() { Face = "Tw3_cardart_special_dummy", Faction=CardFaceFactions.Neutral, Range = CardFaceRanges.Null, Ability = CardFaceAbilities.Decoy,
                    Name="诱饵"},
                new CardFace() { Face = "Tw3_cardart_special_scorch", Faction=CardFaceFactions.Neutral, Range = CardFaceRanges.Null, Ability = CardFaceAbilities.Scorch,
                    Name="烧灼"},
                new CardFace() { Face = "Tw3_cardart_special_horn", Faction=CardFaceFactions.Neutral, Range = CardFaceRanges.Null, Ability = CardFaceAbilities.CommanderHorn,
                    Name="指挥号角"},
                new CardFace() { Face = "Tw3_cardart_neutral_geralt", Faction=CardFaceFactions.Neutral, Range = CardFaceRanges.CloseCombat, Power = 15,
                    Limit=1, Name="利维亚的杰洛特",Hero=true},
                new CardFace() { Face = "Tw3_cardart_neutral_ciri", Faction=CardFaceFactions.Neutral, Range = CardFaceRanges.CloseCombat,Power = 15,
                    Limit=1, Name="希里雅·菲欧娜·伊伦·雷安伦",Hero=true},
                new CardFace() { Face = "Tw3_cardart_neutral_triss", Faction=CardFaceFactions.Neutral, Range = CardFaceRanges.CloseCombat, Power = 7,
                    Limit=1, Name="特莉丝·梅莉葛德",Hero=true},
                new CardFace() { Face = "Tw3_cardart_neutral_yennefer", Faction=CardFaceFactions.Neutral, Range = CardFaceRanges.Ranged, Ability = CardFaceAbilities.Medic, Power = 7,
                    Limit=1, Name="温格堡的叶奈法",Hero=true},
                new CardFace() { Face = "Tw3_cardart_neutral_avallach", Faction=CardFaceFactions.Neutral, Range = CardFaceRanges.CloseCombat, Ability = CardFaceAbilities.Spy, Power = 0,
                    Limit=1, Name="神秘的精灵",Hero=true},
                new CardFace() { Face = "Tw3_cardart_neutral_dandelion", Faction=CardFaceFactions.Neutral, Range = CardFaceRanges.CloseCombat, Ability = CardFaceAbilities.CommanderHorn, Power = 2,
                    Limit=1, Name="丹德里恩"},
                new CardFace() { Face = "Tw3_cardart_neutral_emiel", Faction=CardFaceFactions.Neutral, Range = CardFaceRanges.CloseCombat, Power = 5,
                    Limit=1, Name="爱米尔·雷吉斯·洛赫雷课·塔吉夫"},
                new CardFace() { Face = "Tw3_cardart_neutral_vesemir", Faction=CardFaceFactions.Neutral, Range = CardFaceRanges.CloseCombat, Power = 6,
                    Limit=1, Name="维瑟米尔"},
                new CardFace() { Face = "Tw3_cardart_neutral_villen", Faction=CardFaceFactions.Neutral, Range = CardFaceRanges.CloseCombat, Ability = CardFaceAbilities.Scorch, Power = 7,
                    Limit=1, Name="维纶特瑞坦梅斯"},
                new CardFace() { Face = "Tw3_cardart_neutral_zoltan", Faction=CardFaceFactions.Neutral, Range = CardFaceRanges.CloseCombat, Power = 5,
                    Limit=1, Name="卓尔坦·齐瓦"},
                //new CardFace() { Face = "NR-C1", Range = CardFaceRanges.Null, Ability = CardFaceAbilities.CommanderHorn,
                //    Name="欧吉尔德·伊佛瑞克"},
                //new CardFace() { Face = "NR-C1", Range = CardFaceRanges.Null, Ability = CardFaceAbilities.CommanderHorn,
                //    Name="	刚特·欧迪姆:黑暗"}

                 //if (ability == "CommanderHorn" || ability == "Decoy" || ability == "Scorch" || ability == "ImpenetrableFog" || ability == "TorrentialRain"
                 //       || ability == "ImpenetrableFog" || ability == "BitingFrost" || ability == "SkelligeStorm" || ability == "ClearWeather")
        });

        private void currentCardMouseUp(object sender, MouseButtonEventArgs e)
        {
            // 这里的东西都很不优雅，垃圾，不够MVVM
            TextBlock deckTextblockObj = (TextBlock)((Border)sender).FindName("deckAmountLabel");
            int currentAmount = System.Convert.ToInt32(deckTextblockObj.Text);
            if (deckTextblockObj.Visibility ==  Visibility.Visible)
                deckTextblockObj.Text = currentAmount > 0 ? (currentAmount - 1).ToString() : "0";
            else
            {
                TextBlock remainingTextblockObj = (TextBlock)((Border)sender).FindName("remainingAmountLabel");
                int remainingAmount = System.Convert.ToInt32(remainingTextblockObj.Text);

                int limitNumber = System.Convert.ToInt32(remainingTextblockObj.Tag);
                if(currentAmount < limitNumber)
                    remainingTextblockObj.Text = remainingAmount > 0 ? (remainingAmount - 1).ToString() : "0";
            }
            // 垃圾中的垃圾
            cardModel.refreshInfo();
        }

        private void saveBtnClick(object sender, RoutedEventArgs e)
        {
            //写入原始XML文件
            XmlSerializer serializer = new XmlSerializer(typeof(CardsModel));
            System.IO.FileStream fileStream = new System.IO.FileStream("config.xml", System.IO.FileMode.Create);
            using (fileStream)
            {
                serializer.Serialize(fileStream, cardModel);
            }
            EntranceWindow entranceWin = new EntranceWindow();
            entranceWin.Show();
            this.Close();
        }

        private void windowLocationChanged(object sender, EventArgs e)
        {
            //double offset = leaderPopup.HorizontalOffset;
            //leaderPopup.HorizontalOffset += 1;
            //leaderPopup.HorizontalOffset -= 1;
            //popup.VerticalOffset += 1;
            //popup.VerticalOffset -= 1;
        }

        private void mainGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //leaderPopup.HorizontalOffset += 1;
            //leaderPopup.HorizontalOffset -= 1;
        }

        private void newLeaderSelected(object sender, SelectionChangedEventArgs e)
        {
            this.leadCard.DataContext = leaders[this.leadersFlipView.SelectedIndex > 0 ? this.leadersFlipView.SelectedIndex : 0];
            this.leaderStack.DataContext = leaders[this.leadersFlipView.SelectedIndex > 0 ? this.leadersFlipView.SelectedIndex : 0];
        }
    }
    
}
