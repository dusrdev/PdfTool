namespace PdfTool.Controller;

internal class Adder : IModifier<double> {
    public double Modify(double value, double newValue) {
        return value += newValue;
    }
}