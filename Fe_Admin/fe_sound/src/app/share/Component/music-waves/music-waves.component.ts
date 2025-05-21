import { Component, type ElementRef, type OnDestroy, ViewChild, type AfterViewInit } from "@angular/core"

@Component({
  selector: "app-music-waves",
  template: '<canvas #canvas class="waves-canvas"></canvas>',
  styles: [
    `
    .waves-canvas {
      position: absolute;
      inset: 0;
      width: 100%;
      height: 100%;
      opacity: 0.7;
    }
  `,
  ],
})
export class MusicWavesComponent implements AfterViewInit, OnDestroy {
  @ViewChild("canvas") canvasRef!: ElementRef<HTMLCanvasElement>
  private ctx!: CanvasRenderingContext2D
  private animationFrameId!: number
  private waves: Wave[] = []

  ngAfterViewInit() {
    const canvas = this.canvasRef.nativeElement
    this.ctx = canvas.getContext("2d")!

    this.initCanvas()
    this.createWaves()
    this.animate()

    window.addEventListener("resize", this.handleResize)
  }

  ngOnDestroy() {
    if (this.animationFrameId) {
      cancelAnimationFrame(this.animationFrameId)
    }
    window.removeEventListener("resize", this.handleResize)
  }

  private initCanvas() {
    const canvas = this.canvasRef.nativeElement
    canvas.width = window.innerWidth
    canvas.height = window.innerHeight
  }

  private createWaves() {
    const colors = [
      "rgba(129, 140, 248, 0.2)", // indigo-400
      "rgba(167, 139, 250, 0.15)", // purple-400
      "rgba(196, 181, 253, 0.1)", // violet-300
    ]

    for (let i = 0; i < 3; i++) {
      this.waves.push(new Wave(this.canvasRef.nativeElement, this.ctx, colors[i], 0.5 + i * 0.2, i * 2))
    }
  }

  private animate = () => {
    this.ctx.clearRect(0, 0, this.canvasRef.nativeElement.width, this.canvasRef.nativeElement.height)

    this.waves.forEach((wave) => {
      wave.draw()
    })

    this.animationFrameId = requestAnimationFrame(this.animate)
  }

  private handleResize = () => {
    const canvas = this.canvasRef.nativeElement
    canvas.width = window.innerWidth
    canvas.height = window.innerHeight

    this.waves.forEach((wave) => {
      wave.resize()
    })
  }
}

class Wave {
  private x: number
  private y: number
  private width: number
  private height: number

  constructor(
    private canvas: HTMLCanvasElement,
    private ctx: CanvasRenderingContext2D,
    private color: string,
    private speed: number,
    private offset: number,
  ) {
    this.x = 0
    this.y = canvas.height / 2
    this.width = canvas.width
    this.height = canvas.height
  }

  draw() {
    this.ctx.beginPath()
    this.ctx.moveTo(this.x, this.y)

    const time = Date.now() * 0.001 * this.speed + this.offset

    for (let i = 0; i < this.width; i++) {
      const x = i
      const y =
        this.y +
        Math.sin(x * 0.01 + time) * 50 +
        Math.sin(x * 0.02 + time * 0.8) * 20 +
        Math.sin(x * 0.005 + time * 1.2) * 30

      this.ctx.lineTo(x, y)
    }

    this.ctx.lineTo(this.width, this.height)
    this.ctx.lineTo(this.x, this.height)
    this.ctx.closePath()
    this.ctx.fillStyle = this.color
    this.ctx.fill()
  }

  resize() {
    this.width = this.canvas.width
    this.height = this.canvas.height
    this.y = this.canvas.height / 2
  }
}
