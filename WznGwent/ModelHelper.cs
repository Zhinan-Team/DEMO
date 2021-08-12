using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows;
namespace WznGwent
{
    public enum CardFaceAbilities { Null, Berserker, Medic, Morale, Muster, Scorch, Spy, Bond, TorrentialRain, ImpenetrableFog, BitingFrost, SkelligeStorm,
        ClearWeather, CommanderHorn, Mardroeme, Decoy, MoraleBoost, TightBond
    };
    public enum CardFaceRanges { Null, CloseCombat, Ranged, Siege, Agile };
    public enum CardFaceFactions { Null, Neutral, NorthernRealms, NilfgaardianEmpire, MonstersDeck, ScoiaTael, SkelligeClans };
    public class CardFace : INotifyPropertyChanged
    {
        public string Face { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CardFaceFactions Faction { get; set; }
        public CardFaceRanges Range { get; set; }
        public CardFaceAbilities Ability { get; set; }
        public bool Hero { get; set; }
        private int power = 0;
        public int Power
        {
            get { return power; }
            set
            {
                if (this.power == value) { return; }
                power = value;
                Notify("Power");
            }
        }
        private int amount = 0;
        public int Amount
        {
            get { return amount; }
            set
            {
                if (this.amount == value) { return; }
                amount = value > 3 ? 3 : value;
                Notify("Amount");
            }
        }
        private int limit = 3;
        public int Limit
        {
            get { return limit; }
            set
            {
                if (this.limit == value) { return; }
                limit = value;
                Notify("Limit");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }

    public class CardsModel : INotifyPropertyChanged
    {
        public CardsModel()
        { }

        public CardsModel(CardFace _leaderCard, List<CardFace> _deckcards)
        {
            this.Deckcards = _deckcards;
            this.LeaderCard = _leaderCard;
        }

        // 不需要字段
        public int HeroCardsAmount
        {
            get { return deckcards.Where(c => c.Hero).Sum(c => c.Amount); }
            set { Notify("HeroCardsAmount"); }
        }
        public int AllCardsAmount
        {
            get { return deckcards.Sum(c => c.Amount); }
            set { Notify("AllCardsAmount"); }
        }
        public int UnitCardsAmount
        {
            get { return deckcards.Where(x => x.Range != CardFaceRanges.Null).Sum(c => c.Amount); }
            set { Notify("UnitCardsAmount"); }
        }
        public int MagicCardsAmount
        {
            get { return deckcards.Where(x => x.Range == CardFaceRanges.Null).Sum(c => c.Amount); }
            set { Notify("MagicCardsAmount"); }
        }
        public int PowerAmount
        {
            get { return deckcards.Where(x => x.Range != CardFaceRanges.Null && x.Amount != 0).Sum(c => c.Power * c.Amount); }
            set { Notify("PowerAmount"); }
        }
        public CardFace LeaderCard { get;set; }

        private List<CardFace> deckcards = new List<CardFace>();
        public List<CardFace> Deckcards
        {
            get
            {
                return deckcards;
            }
            set
            {
                if(value != null)
                    deckcards = value;
                //Notify("Deckcards");
            }
        }

        public void refreshInfo()
        {
            AllCardsAmount = 0;
            UnitCardsAmount = 0;
            MagicCardsAmount = 0;
            PowerAmount = 0;
            HeroCardsAmount = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }

    public class imageDirectoryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter != null)
            {
                //理论上，如果用上了参数，从头到底只要一个超级Converter也就行了，这里还是在imageDirectoryConverter语义下的
                //但注意在实际的逻辑中，实现的东西其实从特别的image目录开始，已经没有imageDirectoryConverter的语义抽象了
                switch (System.Convert.ToString(parameter))
                {
                    case "heroCrad":
                        if(System.Convert.ToBoolean(value))
                            return "images\\cardDescriptionHero.png";
                        else
                            return "images\\cardDescription.png";
                    default:
                        return "images\\" + System.Convert.ToString(value) + System.Convert.ToString(parameter) + ".png";

                }
            }
            return "images\\" + System.Convert.ToString(value) + ".png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
    public class toVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // 这里的语义种类就和上面的转换器在不同维度上，就是toVisibilityConverter的反向语义下
            if (parameter != null)
            {
                switch (System.Convert.ToString(parameter))
                {
                    case "reverse":
                        if (System.Convert.ToInt32(value) > 0)
                            return Visibility.Hidden;
                        return Visibility.Visible;
                    //case "faceElements":
                    //    if (System.Convert.ToString(value) == "")
                    //        return Visibility.Hidden;
                    //    else
                    //        return Visibility.Visible;
                    case "heroCardAbility":
                        if (!System.Convert.ToBoolean(value))
                            return Visibility.Hidden;
                        break;
                    case "powerElements":
                        if (System.Convert.ToInt32(value) == 0)
                            return Visibility.Visible;
                        break;
                    case "deck":
                    case "remaining":
                        if(System.Convert.ToString(parameter) == System.Convert.ToString(value))
                            return Visibility.Visible;
                        else
                            return Visibility.Collapsed;
                    default:
                        break;
                }
            }
            //这里其实完全符合了toVisibilityConverter层次的抽象，但上面的reverse破坏了抽象性
            //“=0”契合了大部分枚举类型，即第一个为Null的设计
            if (System.Convert.ToInt32(value) <= 0)
                return Visibility.Hidden;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
    public class toIntStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter != null)
            {
                switch (System.Convert.ToString(parameter))
                {
                    case "normalCardAbility":
                        String ability = System.Convert.ToString(value);
                        if (ability == "CommanderHorn" || ability == "Decoy" || ability == "Scorch" || ability == "ImpenetrableFog" || ability == "TorrentialRain"
                        || ability == "ImpenetrableFog" || ability == "BitingFrost" || ability == "SkelligeStorm" || ability == "ClearWeather")
                            return "0";
                        else
                            return "3";
                    default:
                        return "0";

                }
            }
            return "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
    // 这里名字起的就目标很不抽象，很业务
    public class tabNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            switch (System.Convert.ToString(value))
            {
                case "0":
                    return "所有牌";
                case "1":
                    return "近战单位牌";
                case "2":
                    return "远程单位牌";
                case "3":
                    return "攻城单位牌";
                case "4":
                    return "英雄牌";
                case "5":
                    return "天气牌";
                case "6":
                    return "特殊牌";
                default:
                    return "ERROR";
            }

        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
    public class basedCardWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double actualWidth = System.Convert.ToDouble(value);
            switch (System.Convert.ToString(parameter))
            {
                case "cardText":
                    return System.Convert.ToInt32(actualWidth / 10) == 0 ? 1 : System.Convert.ToInt32(actualWidth / 10);
                case "splitBoard":
                    return "0," + (actualWidth == 0 ? 1 : actualWidth / 50) + ",0,0";
                default:
                    return System.Convert.ToDouble(parameter) * System.Convert.ToDouble(value);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
    public class cardWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double actualWidth = System.Convert.ToDouble(value) - 240;
            return actualWidth < 0 ? 0 : actualWidth / 6;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
    public class remainingCardsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 3 - System.Convert.ToInt32(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 3 - System.Convert.ToInt32(value);
        }
    }
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (System.Convert.ToString(parameter))
            {
                case "reverse":
                    if (System.Convert.ToBoolean(value))
                    {
                        return "White";
                    }
                    return "Black";
                default:
                    if (System.Convert.ToBoolean(value) )
                    {
                        return "Black";
                    }
                    return "White";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
    public class InfoColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (System.Convert.ToString(parameter))
            {
                case "units":
                    if (System.Convert.ToInt32(value) < 22)
                    {
                        return "#BA2A28";
                    }
                    break;
                default:
                    if (System.Convert.ToInt32(value) > 10)
                    {
                        return "#BA2A28";
                    }
                    break;
            }
            return "#827A6E";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
    
    public class RemainingCardsShowConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Count() >= 2)
            {
                if(System.Convert.ToString(values[2])=="remaining")
                    return ((int)values[1] - (int)values[0]).ToString();
            }
            return System.Convert.ToString(values[0]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class AbilityEllipseRowConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Count() >= 1)
            {
                String ability = System.Convert.ToString(values[0]);
                String range = System.Convert.ToString(values[1]);
                if (range == "Null" && (ability == "CommanderHorn" || ability == "Decoy" || ability == "Scorch" || ability == "ImpenetrableFog" || ability == "TorrentialRain"
                || ability == "ImpenetrableFog" || ability == "BitingFrost" || ability == "SkelligeStorm" || ability == "ClearWeather"))
                    return 0;
            }
            return 3;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class CardCollapsedConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Count() >= 2 )
            {
                if (values[2].ToString() == "remaining")
                {
                    if (values[0].ToString() == values[1].ToString())
                        return Visibility.Collapsed;
                    else
                        return Visibility.Visible;
                }
                else
                {
                    if (values[0].ToString() == "0")
                        return Visibility.Collapsed;
                    else
                        return Visibility.Visible;
                }
            }
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class LimitTipVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Count() >= 2)
            {
                if (values[0].ToString() == values[1].ToString() && values[2].ToString() == "remaining")
                    return Visibility.Visible;
            }
            return Visibility.Hidden;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BlurConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Count() >= 1)
            {
                if (System.Convert.ToString(values[0]) == "#FFBA2A28" || System.Convert.ToString(values[1]) == "#FFBA2A28")
                    return 20.0;
            }
            return 0.0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class SaveEnablityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Count() >= 1)
            {
                if (System.Convert.ToString(values[0]) == "#FFBA2A28" || System.Convert.ToString(values[1]) == "#FFBA2A28")
                    return false;
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
