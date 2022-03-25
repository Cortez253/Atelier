using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atelier
{
    public partial class Clients
    {
        public override string ToString()
        {
            string FullName = Surname + " " + Name + " " + Patronymic;
            return FullName;
        }
    }

    public partial class Materials
    {
        public override string ToString()
        {
            return Name_material;
        }
    }

    public partial class Products
    {
        public override string ToString()
        {
            return Name_product;
        }
    }

    public partial class Workers
    {
        public override string ToString()
        {
            string FullName = Surname + " " + Name + " " + Patronymic;
            return FullName;
        }
    }

    public partial class Users
    {
        public override string ToString()
        {
            return Login;
        }
    }

    public partial class Orders
    {
        public override string ToString()
        {
            return Chest_girth.ToString() + Waist_girth;
        }
    }
}
