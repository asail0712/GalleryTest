#import "Sample.h"

@implementation PhotoCatcherManager

static PhotoCatcherManager *sharedInstance = nil;

+ (instancetype)sharedManager 
{
    static dispatch_once_t onceToken;

    dispatch_once(
        &onceToken, 
        ^{
            sharedInstance = [[self alloc] init];
        }
    );

    return sharedInstance;
}

- (void)checkAuthorizationStatus
{
    PHAuthorizationStatus authorizationStatus = [PHPhotoLibrary authorizationStatus];
    
    if (authorizationStatus != PHAuthorizationStatusAuthorized) 
    {
        //已获得授权 doSomthing
        [self getAllPhoto];
    }
    else if (authorizationStatus == PHAuthorizationStatusNotDetermined) 
    { 
        // 没有授权过，弹出授权请求
        [PHPhotoLibrary requestAuthorization:
            ^(PHAuthorizationStatus status) 
            {
                if (status == PHAuthorizationStatusAuthorized) 
                {
                    // 用户选择授权 doSomthing
                    [self getAllPhoto];
                } 
                else 
                {
                    // 用户选择拒绝 提示去设置界面 授权相册
                    NSLog(@"请在设置界面, 授权访问相册");
                }
            }
        ];
    }
}  

- (void)getAllPhoto
{
        [self getAllAssetInPhotoAlbumAsync:YES completion:
            ^(NSArray<PHAsset *> *assets) 
            {
                // 在這裡處理獲取到的照片資源數組
                for (PHAsset *asset in assets) 
                {
                    // 進行相應的處理
                    NSLog(@"照片名%@", [asset valueForKey:@"filename"]);

                    //UnitySendMessage( "PhotoLibraryController" , "ReceiveThumbnail", [asset valueForKey:@"filename"]);
                }

                [self processPhotos:assets];
            }
        ];
}

- (void)processPhotos:(NSArray<PHAsset *> *)assets
{
    dispatch_async(
        dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0), 
        ^{
            // 在這裡處理照片資源
            // ...        
            dispatch_async(
                dispatch_get_main_queue(), 
                ^{
                // 返回結果或執行其他主線程操作
                // ...
                }
            );
        }
    );
}

#pragma mark - 非同步获取相册内所有照片资源 
- (void)getAllAssetInPhotoAlbumAsync:(BOOL)ascending completion:(void (^)(NSArray<PHAsset *> *assets))completion 
{
    NSMutableArray<PHAsset *>* assets   = [NSMutableArray array];      
    PHFetchOptions* option              = [[PHFetchOptions alloc] init];
    option.sortDescriptors              = @[[NSSortDescriptor sortDescriptorWithKey:@"creationDate" ascending:ascending]];
      
    PHFetchResult *result               = [PHAsset fetchAssetsWithMediaType:PHAssetMediaTypeImage options:option];
      
    dispatch_async(
        dispatch_get_global_queue(
            DISPATCH_QUEUE_PRIORITY_DEFAULT, 
            0
        ), 
        ^{
            [result enumerateObjectsUsingBlock:
                ^(PHAsset *asset, NSUInteger idx, BOOL *stop) 
                {
                    [assets addObject:asset];
                }
            ];
        
            dispatch_async(
                dispatch_get_main_queue(), 
                ^{
                    completion(assets);
                }
            );
        }
    );
}

#pragma mark - 同步获取相册内所有照片资源  
/*
- (NSArray<PHAsset *> *)getAllAssetInPhotoAblum:(BOOL)ascending  
{  
    NSMutableArray<PHAsset *> *assets = [NSMutableArray array];  
      
    PHFetchOptions *option = [[PHFetchOptions alloc] init];  
    //ascending 为YES时，按照照片的创建时间升序排列;为NO时，则降序排列  
    option.sortDescriptors = @[[NSSortDescriptor sortDescriptorWithKey:@"creationDate" ascending:ascending]];  
      
    PHFetchResult *result = [PHAsset fetchAssetsWithMediaType:PHAssetMediaTypeImage options:option];  
      
    [result enumerateObjectsUsingBlock:^(id  _Nonnull obj, NSUInteger idx, BOOLBOOL * _Nonnull stop) {  
        PHAsset *asset = (PHAsset *)obj;  
        NSLog(@"照片名%@", [asset valueForKey:@"filename"]);  
        [assets addObject:asset];  
    }];  
      
    return assets;  
}  */

@end
/*
extern "C"
{   
    void GetPhotosThumbnails()
    {
        UnitySendMessage( "PhotoLibraryController" , "ReceiveThumbnail", "Test String~~!!!");
    }
}*/
extern "C" 
{
    void CheckAuthorizationStatus() 
    {
        [[PhotoCatcherManager sharedManager] checkAuthorizationStatus];
    }
}