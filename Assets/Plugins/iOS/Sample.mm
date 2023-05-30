extern "C" {
    // C#函数的函数指针
    typedef void (*cs_callback)(int);

    void sampleMethod4(cs_callback callback) {
        callback(9);
    }
}