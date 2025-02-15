public interface IStrategy {
    void Process(string path);

    string TranspileDataStructure(string dataStructure);
}