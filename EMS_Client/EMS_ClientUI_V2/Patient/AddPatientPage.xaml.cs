using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EMS_ClientUI_V2
{

    /// <summary>
    /// Interaction logic for AddNewPatient.xaml
    /// </summary>
    public partial class AddPatient : Page
    {
        Regex nameValidator;
        Regex healthCardValidator;
        Regex initialValidator;
        Regex dobValidator;
        Regex phoneNumValidator;

        public AddPatient()
        {
            InitializeComponent();

            nameValidator = new Regex(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$");
            healthCardValidator = new Regex(@"^\d{10}$");
            initialValidator = new Regex(@"^[a-zA-Z]{1}$");
            dobValidator = new Regex(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$");
            phoneNumValidator = new Regex(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$");

            fNameValid.Visibility = Visibility.Hidden;//done
            lNameValid.Visibility = Visibility.Hidden;//done
            healthCardValid.Visibility = Visibility.Hidden;//done
            initialValid.Visibility = Visibility.Hidden;//done
            dobValid.Visibility = Visibility.Hidden;//done
            genderValid.Visibility = Visibility.Hidden;//done
            hhValid.Visibility = Visibility.Hidden;//done
            cityValid.Visibility = Visibility.Hidden;//done
            phoneValid.Visibility = Visibility.Hidden;


            genderBox.Items.Add("Male");
            genderBox.Items.Add("Female");
            genderBox.Items.Add("Other");
            genderBox.SelectedIndex = 0;
            provinceBox.Items.Add("AB");
            provinceBox.Items.Add("BC");
            provinceBox.Items.Add("MB");
            provinceBox.Items.Add("NB");
            provinceBox.Items.Add("NL");
            provinceBox.Items.Add("NS");
            provinceBox.Items.Add("NT");
            provinceBox.Items.Add("NU");
            provinceBox.Items.Add("ON");
            provinceBox.Items.Add("PE");
            provinceBox.Items.Add("QC");
            provinceBox.Items.Add("SK");
            provinceBox.Items.Add("YT");
            provinceBox.SelectedIndex = 0;

        }
        private void AddNewPatientBtn_Click(object sender, RoutedEventArgs e)
        {
            int checkValues = 0;//Must equal 8 to continue

            if (nameValidator.IsMatch(firstNameTxtBox.Text))
            {
                checkValues++;
                fNameValid.Visibility = Visibility.Hidden;
            }
            else
            {
                fNameValid.Visibility = Visibility.Visible;
            }

            if (nameValidator.IsMatch(lastNameTxtBox.Text))
            {
                checkValues++;
                lNameValid.Visibility = Visibility.Hidden;
            }
            else
            {
                lNameValid.Visibility = Visibility.Visible;
            }

            if (nameValidator.IsMatch(cityTxtBox.Text))
            {
                checkValues++;
                cityValid.Visibility = Visibility.Hidden;
            }
            else
            {
                cityValid.Visibility = Visibility.Visible;
            }

            if (initialValidator.IsMatch(middleInitialTxtBox.Text))
            {
                checkValues++;
                initialValid.Visibility = Visibility.Hidden;
            }
            else
            {
                initialValid.Visibility = Visibility.Visible;
            }

            if (healthCardValidator.IsMatch(healthCardTxtBox.Text))
            {
                checkValues++;
                healthCardValid.Visibility = Visibility.Hidden;
            }
            else
            {
                healthCardValid.Visibility = Visibility.Visible;
            }

            if (nameValidator.IsMatch(headHouseTxtBox.Text))
            {
                checkValues++;
                hhValid.Visibility = Visibility.Hidden;
            }
            else if (headHouseTxtBox.Text == "")
            {
                checkValues++;
                hhValid.Visibility = Visibility.Hidden;
            }
            else
            {
                hhValid.Visibility = Visibility.Visible;
            }

            if (dobValidator.IsMatch(dobTxtBox.Text))
            {
                checkValues++;
                dobValid.Visibility = Visibility.Hidden;
            }
            else
            {
                dobValid.Visibility = Visibility.Visible;
                
            }

            if (phoneNumValidator.IsMatch(phoneNumTxtBox.Text))
            {
                checkValues++;
                phoneValid.Visibility = Visibility.Hidden;
            }
            else
            {
                phoneValid.Visibility = Visibility.Visible;
            }
        }
    }
}
