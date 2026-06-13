package main

import (
	"fmt"
	"log/slog"
	"os"
	"sync"

	"github.com/nats-io/nats.go"
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

	nc.Subscribe(">", func(msg *nats.Msg) {
		slog.Info("received message", "message", string(msg.Data))
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
