extern "C"
{   
    void GetPhotosThumbnails()
    {
        UnitySendMessage( "PhotoLibraryController" , "ReceiveThumbnail", "Test String~~!!!");
    }
}
