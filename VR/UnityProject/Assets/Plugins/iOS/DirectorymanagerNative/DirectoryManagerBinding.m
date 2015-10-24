//
//  FileManagerBinding.m
//  Unity-iPhone
//
//  Created by Patric Schmid on 29/09/14.
//
//

// Converts NSString to C style string by way of copy (Mono will free it)
#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

// Converts C style string to NSString as long as it isnt empty
#define GetStringParamOrNil( _x_ ) ( _x_ != NULL && strlen( _x_ ) ) ? [NSString stringWithUTF8String:_x_] : nil

const char * _getCachesPath(){
    NSURL* url = [[NSFileManager defaultManager] URLsForDirectory:NSCachesDirectory inDomains:NSUserDomainMask][0];
    return MakeStringCopy(url.path);
}

const char *_getLibraryPath(){
    NSURL* url = [[NSFileManager defaultManager] URLsForDirectory:NSLibraryDirectory inDomains:NSUserDomainMask][0];
    return MakeStringCopy(url.path);
}

const char *_getAppSupportPath(){
    NSURL* url = [[NSFileManager defaultManager] URLsForDirectory:NSApplicationSupportDirectory inDomains:NSUserDomainMask][0];
    return MakeStringCopy(url.path);
}

const char *_getTempPath(){
    NSString* urlstring = NSTemporaryDirectory();
    return MakeStringCopy(urlstring);
}