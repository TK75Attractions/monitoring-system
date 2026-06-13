package main

import (
	"fmt"
	"log"
	"log/slog"
	"os"
	"sync"

	monitoringv1 "github.com/TK75Attractions/monitoring-system/gen"
	"github.com/nats-io/nats.go"
	"google.golang.org/protobuf/proto"
)

const NATS_IP_ADDR = "127.0.0.1"

func main() {
	initLog()

	natsUrl := fmt.Sprintf("%v:%v", NATS_IP_ADDR, nats.DefaultPort)

	slog.Info("connecting to NATS server", "url", natsUrl)
	nc, err := nats.Connect(natsUrl)
	if err != nil {
		slog.Error("failed to connect to NATS server", "error", err)
		return
	}
	defer nc.Close()
	slog.Info("succeed to connect to NATS server")

	wg := sync.WaitGroup{}
	wg.Add(1)

	nc.Subscribe("alert.system-stats.battery", func(msg *nats.Msg) {
		packet := &monitoringv1.LogPacket{}
		if err := proto.Unmarshal(msg.Data, packet); err != nil {
			log.Fatalf("failed to unmarshal LogPacket: %v", err)
		}
		slog.Info("received message", "message", packet)
		wg.Done()
	})
	wg.Wait()
}

func initLog() {
	var logLevel = new(slog.LevelVar)
	h := slog.NewTextHandler(os.Stderr, &slog.HandlerOptions{Level: logLevel})
	slog.SetDefault(slog.New(h))
	logLevel.Set(slog.LevelDebug)
}
