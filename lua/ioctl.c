#include <sys/ioctl.h>
#include <fcntl.h>

#include "lua.h"
#include "lauxlib.h"

static int fcntl_ioctl (lua_State *L) {
    lua_pushnumber(L, 42);
    return 1;
};

static const struct luaL_Reg fcntl_lib[] = {
    {"ioctl", fcntl_ioctl},
    {NULL,    NULL       }
};

int luaopen_luafcntl (lua_State *L) {
  luaL_newlibtable(L, fcntl_lib);
  luaL_setfuncs(L, fcntl_lib, 0);
  return 1;
};
    
// int main(int argc, char* argv[])
// {
//     int serial;
//     int fd = open("/dev/ttyS0", O_RDONLY);
//     ioctl(fd, TIOCMGET, &serial);
//     if (serial & TIOCM_DTR)
//         puts("TIOCM_DTR is not set");
//     else
//         puts("TIOCM_DTR is set");
//     close(fd);
// } 
