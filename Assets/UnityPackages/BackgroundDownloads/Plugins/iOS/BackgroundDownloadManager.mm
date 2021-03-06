#import "BackgroundDownloadManager.h"

// See reference documentation at: https://developer.apple.com/documentation/foundation/url_loading_system/downloading_files_in_the_background?language=objc

@implementation BackgroundDownloadManager

#define SESSION_ID "com.xorhead.unity.downloading"

static NSURLSession *backgroundSession;
static BackgroundDownloadManager *_instance;
static NSString *path;

+ (BackgroundDownloadManager *) instance {
    
    if (_instance == nil){
        _instance = [[BackgroundDownloadManager alloc] init];
    }
    
    return _instance;
}

-(id) init {
    
    if(self = [super init]) 
    {
        pendingDownloadTasks = [[NSMutableDictionary alloc] init];
        
        [self initDownloadSession];
    }

    return self;
}

- (void) initDownloadSession
{
    // Create a session configuration object.
    NSURLSessionConfiguration *backgroundConfiguration = [NSURLSessionConfiguration backgroundSessionConfigurationWithIdentifier: [NSString stringWithUTF8String:SESSION_ID]];
    
    id <NSURLSessionDelegate> delegate = [[BackgroundDownloadDelegate alloc] init];
    
    // Create a session
    backgroundSession = [NSURLSession sessionWithConfiguration:backgroundConfiguration delegate:delegate delegateQueue:nil];
    
    // Retrieve all existing tasks from the session.
    [backgroundSession getTasksWithCompletionHandler:^(NSArray *dataTasks, NSArray *uploadTasks, NSArray *downloadTasks)
    {
        NSUInteger count = [downloadTasks count];
        
        if (count > 0)
        {
            for (NSURLSessionDownloadTask *dl in downloadTasks)
            {
                [self addDownloadTask:dl];
            }
        }
    }];
}

- (void) setDownloadPath:(NSString *) storagePath
{
    path = storagePath;
}

- (long) startDownload:(NSString *) url
{
    // Create a download task
    NSURL *downloadUrl = [NSURL URLWithString:url];
    
    NSURLSessionDownloadTask *downloadTask = [backgroundSession downloadTaskWithURL:downloadUrl];
    [downloadTask resume];
    
    // Add task to tracked download tasks
    [self addDownloadTask:downloadTask];

    return downloadTask.taskIdentifier;
}

- (void) cancelDownloadTask:(long) identifier
{
    NSURLSessionTask *task = [self getDownloadTask:identifier];
    
    if (task == NULL)
    {
        // TODO: log ??
        
        return;
    }
    
    if (task.state == NSURLSessionTaskStateRunning)
    {
        [task cancel];
        
        [self removeDownloadTaskById:identifier];
    }
    else
    {
        // TODO: log ??
    }
}

- (void) addDownloadTask:(NSURLSessionTask *) downloadTask
{
    [pendingDownloadTasks setObject:downloadTask forKey:[NSNumber numberWithLong:downloadTask.taskIdentifier]];
}

- (void) removeDownloadTask:(NSURLSessionTask *) downloadTask
{
    [self removeDownloadTaskById:downloadTask.taskIdentifier];
}

- (void) removeDownloadTaskById:(long) identifier
{
    NSURLSessionTask *task = [pendingDownloadTasks objectForKey:[NSNumber numberWithLong:identifier]];

    if (task != NULL)
    {
        [pendingDownloadTasks removeObjectForKey:[NSNumber numberWithLong:identifier]];
    }
}

- (NSURLSessionTask *) getDownloadTask:(long) identifier
{
    NSURLSessionTask *transaction = [pendingDownloadTasks objectForKey:[NSNumber numberWithLong:identifier]];
    
    return transaction;
}

- (NSURLSessionTask *) getDownloadTaskWithURL:(NSString *) url
{
    for(id key in pendingDownloadTasks) {
        NSURLSessionTask *task = pendingDownloadTasks[key];
        
        if ([task.originalRequest.URL.absoluteString isEqualToString:url]) {
            return task;
        }
    }
    
    return NULL;
}

- (void) moveDownloadToDestination:(NSURL *) tempFileURL downloadedFilename:(NSString *) filename
{
    // Move the file to a new URL
    NSFileManager *fileManager = [NSFileManager defaultManager];
    NSError *moveError = nil;

    NSArray *components = [NSArray arrayWithObjects:path, filename, nil];
    NSString *destFullPath = [NSString pathWithComponents:components];
    
    if ([fileManager fileExistsAtPath:destFullPath])
    {
        [fileManager removeItemAtPath:destFullPath error:&moveError];
    }
    
    if ([fileManager moveItemAtURL:tempFileURL toURL:[NSURL fileURLWithPath:destFullPath isDirectory:FALSE] error:&moveError])
    {
    }
    else
    {
        // TODO: log an error ?
    }
}

@end