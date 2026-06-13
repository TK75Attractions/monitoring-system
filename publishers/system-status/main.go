package main

import (
	"fmt"
	"log/slog"
	"os"
	"time"

	monitoringv1 "github.com/TK75Attractions/monitoring-system/gen"
	"github.com/nats-io/nats.go"
	"google.golang.org/protobuf/proto"
)

const NatsIpAddr = "127.0.0.1"

func main() {
	initLog()

	natsUrl := fmt.Sprintf("%v:%v", NatsIpAddr, nats.DefaultPort)
	nc, err := connectNats(natsUrl)
	if err != nil {
		slog.Error("failed to connect to NATS server: %v\n", "error", err)
	}
	defer nc.Close()

	watchBatteryPercentage(nc)
}

func initLog() {
	var logLevel = new(slog.LevelVar)
	h := slog.NewTextHandler(os.Stderr, &slog.HandlerOptions{Level: logLevel})
	slog.SetDefault(slog.New(h))
	logLevel.Set(slog.LevelDebug)
}

func connectNats(url string) (*nats.Conn, error) {
	slog.Info("connecting to NATS server", "url", url)
	nc, err := nats.Connect(url)
	if err != nil {
		return nil, err
	}
	slog.Info("succeed to connect to NATS server")
	return nc, nil
}

func watchBatteryPercentage(nc *nats.Conn) {
	//ticker := time.NewTicker(1 * time.Minute)
	//defer ticker.Stop()

	//for range ticker.C {
	p := getBatteryPercentage()
	//if p < 20 {
	packet := monitoringv1.LogPacket{
		Level:     monitoringv1.LogLevel_LOG_LEVEL_WARN,
		Message:   fmt.Sprintf("battery percentage is %v", p),
		OccuredAt: time.Now().Unix(),
	}
	out, err := proto.Marshal(&packet)
	if err != nil {
		panic("infallible")
	}
	nc.Publish("alert.system-stats.battery", out)
	//}
	//}
}
