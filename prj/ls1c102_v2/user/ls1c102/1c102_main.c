
#include "ls1x.h"
#include "Config.h"
#include "ls1x_gpio.h"
#include "ls1x_latimer.h"


int main(int arg, char *args[])
{
    while(1)
    {

        gpio_write_pin(GPIO_PIN_20, 1);
        delay_ms(500);
        gpio_write_pin(GPIO_PIN_20, 0);
        delay_ms(500);

    }
    return 0;
}
