import { Component } from '@angular/core';

@Component({
  selector: 'app-music-waves',
  template: `
    <div class="music-waves">
      <div class="wave" *ngFor="let i of [0,1,2,3,4,5,6,7,8,9]" [style.height.px]="getWaveHeight(i)"></div>
    </div>
  `,
  styles: [`
    .music-waves {
      position: absolute;
      bottom: 0;
      left: 0;
      right: 0;
      height: 100px;
      display: flex;
      align-items: flex-end;
      justify-content: center;
      gap: 3px;
      padding: 0 20px;
      overflow: hidden;
    }

    .wave {
      width: 3px;
      background: rgba(255, 255, 255, 0.2);
      border-radius: 3px;
      animation: wave 2s ease-in-out infinite;
    }

    @keyframes wave {
      0%, 100% { transform: scaleY(1); }
      50% { transform: scaleY(0.5); }
    }
  `]
})
export class MusicWavesComponent {
  getWaveHeight(index: number): number {
    return Math.sin(index / 5) * 8 + 10;
  }
}
