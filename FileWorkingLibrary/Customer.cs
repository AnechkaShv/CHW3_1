

namespace FileWorkingLibrary
{
    public class Customer: IComparable<Customer>
    {
        public readonly int id, age;
        public readonly string? name, email, city;
        public readonly bool isPremium;
        public readonly double[] orders;
        public Customer() 
        { 
            id = 0; age = 0;
            name = null; email = null; city = null;
            isPremium = false;
            orders = null;
        }
        /// <summary>
        /// This constructor initialize variables with input values.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="age"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="city"></param>
        /// <param name="orders"></param>
        /// <param name="isPremium"></param>
        public Customer(int id, int age, string name, string email, string city, double[] orders, bool isPremium)
        {
            this.id = id;
            this.age = age;
            this.name = name;
            this.email = email;
            this.city = city;
            this.orders = orders;
            this.isPremium = isPremium;
        }

        /// <summary>
        /// This method compares values of "orders"
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Customer? other)
        {
            if (other == null) 
                return 1;

            // At first, compare orders' length.
            if (this.orders.Length.CompareTo(other.orders.Length) != 0)
                return this.orders.Length.CompareTo(other.orders.Length);

            // If lengths are equal comparing elements.
            for (int i = 0; i < this.orders.Length; i++)
            {
                if (this.orders[i].CompareTo(other.orders[i]) != 0)
                    return this.orders[i].CompareTo(other.orders[i]);
            }
            return 0;
        }
        /// <summary>
        /// This method compares all fields except "orders".
        /// </summary>
        /// <param name="other"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int CompareTo(Customer? other, int idx)
        {
            if(other == null) 
                return 1;

            // Comparing all values except orders.
            return idx switch
            {
                1 => this.id.CompareTo(other.id),
                2 => this.name.CompareTo(other.name),
                3 => this.email.CompareTo(other.email),
                4 => this.age.CompareTo(other.age),
                5 => this.city.CompareTo(other.city),
                6 => this.isPremium.CompareTo(other.isPremium),
                _ => throw new NotImplementedException()
            };
        }
        /// <summary>
        /// This method transforms customer's data to the json format.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // Values and keys to printing.
            string[] names = new string[] {"customer_id", "name", "email", "age", "city", "is_premium"};
            string[] values = new string[] { id.ToString(), $"\"{name}\"", $"\"{email}\"", age.ToString(), $"\"{city}\"", $"\"{isPremium}\"" };

            // Forming a string with data in json format.
            string result = "  {\n";

            // Writing all keys, values except orders.
            for (int i = 0; i < names.Length; i++)
            {
                result += $"    \"{names[i]}\": {values[i]},\n";
            }
            result += "    \"orders\": [\n";

            // Writing orders data in the right way.
            for (int i = 0; i < orders.Length; i++)
            {
                if(i != orders.Length - 1)
                    result += $"      {orders[i]},\n";
                else
                    result += $"      {orders[i]}\n";
            }
            result += $"    ]\n  }},\n";
            return result;
        }
    }
}