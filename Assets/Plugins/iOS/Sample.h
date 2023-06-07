#import <Foundation/Foundation.h>
#import <Photos/Photos.h>

extern "C"
{
	void CheckAuthorizationStatus();
	void SendMessage(const char*);
}

@interface PhotoCatcherManager : NSObject

+ (instancetype)sharedManager;

-(void)checkAuthorizationStatus;
-(void)getAllPhoto;
-(void)processPhotos:(NSArray<PHAsset*> *)assets;
-(void)getAllAssetInPhotoAlbumAsync:(BOOL)ascending completion : (void(^)(NSArray<PHAsset*> *assets))completion;
//-(NSArray<PHAsset*> *)getAllAssetInPhotoAlbum:(BOOL)ascending;

@end