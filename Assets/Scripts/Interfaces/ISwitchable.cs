interface ISwitchable
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