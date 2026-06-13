package main

import (
	"log/slog"
	"os"
	"strconv"
)

const BATTERY_CAP_FILE_NAME = "/sys/class/power_supply/BAT0/capacity"

func getBatteryPercentage() uint8 {
	f, err := os.Open(BATTERY_CAP_FILE_NAME)
	if err != nil {
		slog.Error("can't open battery capacity file", "filename", BATTERY_CAP_FILE_NAME, "err", err)
		panic("") //TODO: これは回復可能か？
	}
	defer f.Close()

	data := make([]byte, 3)
	_, err = f.Read(data)
	if err != nil {
		slog.Error("can't read battery capacity from file", "filename", BATTERY_CAP_FILE_NAME, "err", err)
	}

	ret, err := strconv.ParseUint(string(data), 10, 8)
	if err != nil {
		panic("content of capacity file must be integar")
	}

	return uint8(ret)
}
