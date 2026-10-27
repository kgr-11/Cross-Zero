using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cross_Zero
{
    public  class Neuron
    {
        double y;//Текущий выход нейрона
        public double Y
        {
            get { return y; }
        }

        //функция-активации, обновление значения выхода нейрона
        public void Activate(double x)
        {
            //y = 1.0 / (1 + Math.Exp(-x));
            y = Math.Tanh(x);

        }
        //производная функции активации
        public double DerivateY()
        {
            return 1.0 - y * y;
        }
    }
}
