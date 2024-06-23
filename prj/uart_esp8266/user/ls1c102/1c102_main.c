
#include "ls1x.h"
#include "Config.h"
#include "ls1x_gpio.h"
#include "ls1x_latimer.h"
#include "esp8266.h"
#include "ls1c102_interrupt.h"

#define LED 20
/* 1c102的uart端口
    UART0(连接外部ESP8266)
    gpio_pin_remap(GPIO_PIN_6,GPIO_FUNC_MAIN);          //GPIO6管脚复用为RX
    gpio_pin_remap(GPIO_PIN_7,GPIO_FUNC_MAIN);          //GPIO7管脚复用为TX
    
    UART1(连接板上CH341A)
    gpio_pin_remap(GPIO_PIN_8,GPIO_FUNC_MAIN);          //GPIO8管脚复用为RX
    gpio_pin_remap(GPIO_PIN_9,GPIO_FUNC_MAIN);          //GPIO9管脚复用为TX

    UART2(连接板上CH341A)
    gpio_pin_remap(GPIO_PIN_38,2);                      //GPIO38管脚复用为RX
    gpio_pin_remap(GPIO_PIN_39,2);                      //GPIO39管脚复用为TX
*/
int main(int arg, char *args[])
{

    esp8266_init();

    while(1)
    {
        delay_ms(1000);
        esp8266_send_data("Hello World!");
    }

    return 0;
}
