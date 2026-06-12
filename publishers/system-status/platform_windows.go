package main

import (
	"fmt"
	"os"
	"unsafe"

	"golang.org/x/sys/windows"
)

type SystemPowerStatus struct {
	ACLineStatus        uint8
	BatteryFlag         uint8
	BatteryLifePercent  uint8
	SystemStatusFlag    uint8
	BatteryLifeTime     uint32
	BatteryFullLifeTime uint32
}

// Returns percentage of remainig battery.
func getBatteryPercentage() uint8 {
	proc := windows.NewLazySystemDLL("kernel32.dll").NewProc("GetSystemPowerStatus")
	var s SystemPowerStatus

	ret1, _, err := proc.Call(uintptr(unsafe.Pointer(&s)))
	if ret1 != 0 {
		fmt.Fprintf(os.Stderr, "error occured while calling GetSystemPowerStatus(): %s\n", err)
	}
	return s.BatteryLifePercent
}
