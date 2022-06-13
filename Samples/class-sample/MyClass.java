import java.io.File;

public class MyClass {
    public final Double myProperty = 3.0;
    private int value;

    private MyClass(String argument) {
    }

    public MyClass() {
    }

    public static void main(String[] args) {
    }

    public int getValue() {
        return value;
    }

    public void setValue(int thisValue, int value2, String[] files, File myFile) {
        value = thisValue;
    }
}