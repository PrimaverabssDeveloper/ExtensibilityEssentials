using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
$endif$using System.Text;
$if$ ($targetframeworkversion$ >= 4.5)using System.Threading.Tasks;
$endif$using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.CustomForm;

namespace $rootnamespace$
{
    public partial class $safeitemrootname$ : CustomForm
    {
        public $safeitemrootname$()
        {
            InitializeComponent();
        }
    }
}
