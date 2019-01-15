interface ISwitch
{
    // Property signatures:
    bool isOn{
        get;
        set;
    }
    void switchOn();

    void switchOff();

    void toggle();
}