import { Component, AfterViewInit, NgZone,Renderer2, OnDestroy } from '@angular/core';
import * as THREE from 'three';
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader.js'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';
import { SharedService } from '../services/shared.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { LoginStatusService } from '../services/login-status.service';

@Component({
  selector: 'app-try-editor',
  templateUrl: './try-editor.component.html',
  styleUrl: './try-editor.component.css'
})
export class TryEditorComponent implements AfterViewInit, OnDestroy{
  //boolean to show/hide the whole html component
  showTryEditor = true;
  private switchToEditorSubscription!: Subscription; //subscribe to the Obervable in the service 

  //Can also create a seperate service for this
  private scene!: THREE.Scene;
  private camera!: THREE.PerspectiveCamera;
  private renderer!: THREE.WebGLRenderer;
  private model: any;
  private controls!: OrbitControls;
  private sceneContainer!: HTMLElement;

  //constructor with all required services injected
  constructor(private ngZone: NgZone,
    private renderer2: Renderer2,
    private sharedService : SharedService,
    private router : Router,
    private loginStatus : LoginStatusService
    ){
      this.switchToEditorSubscription = this.sharedService.switchToEditor$.subscribe(() => {
        this.showTryEditor = false;
    })
  }

  //unsubscribe the Subscription
  ngOnDestroy() {
  this.switchToEditorSubscription.unsubscribe();
  }

  //Using ngAfterViewInit since I am using 3d models (views)
  ngAfterViewInit(){
    this.sceneContainer = this.renderer2.selectRootElement('#scene-container');
    this.initScene();
    this.loadModel();
    this.initOrbitControls();
    this.animate();
  }

  //initialise the scene
  private initScene(){
    this.scene = new THREE.Scene();
    this.camera = new THREE.PerspectiveCamera(
      35,
      window.innerWidth / window.innerHeight,
      0.1,
      1000
    );
    this.camera.position.set(20,3,5);
    this.scene.add(this.camera); //Camera added to scene

    this.renderer = new THREE.WebGLRenderer({ antialias: true });
    this.renderer.shadowMap.enabled = true; // Enable shadow mapping

    // Set the clear color with an alpha value
    this.renderer.setClearColor(0x000000, 0); // 0x000000 is black, 0 is fully transparent

    this.renderer.setSize(window.innerWidth, 800);


    this.renderer2.appendChild(this.sceneContainer, this.renderer.domElement);

    const light = new THREE.AmbientLight(0xffffff, 1);
    this.scene.add(light)
  }

  //Orbit controls for the 3d model (allow movements of the model)
  private initOrbitControls() {
    this.controls = new OrbitControls(this.camera, this.renderer.domElement);
    this.controls.enableDamping = true; // an animation loop is required when either damping or auto-rotation are enabled
    this.controls.dampingFactor = 0.25;
    this.controls.screenSpacePanning = false;
    this.controls.maxPolarAngle = Math.PI / 2;
  }

  //load the model
  private loadModel() {
    const loader = new GLTFLoader();

    loader.load('../../assets/desktop_pc/scene.gltf', (gltf) => {
      this.model = gltf.scene;
      this.scene.add(this.model);
    });
  }

  //animate the model
  private animate() {
    this.ngZone.runOutsideAngular(() => {
  
      const animateFn = () => {
        requestAnimationFrame(animateFn);
        this.controls.update();
        this.renderer.render(this.scene, this.camera);
      };

      animateFn();
    });
  }

  //Navigate to appropriate component while checking the loggedin Status
  switchToEditor() {
    
    this.loginStatus.getLoginStatusObservable().subscribe((response)=>{
      this.sharedService.emitSwitchToEditor(); //remove this component from the app 
      if(response || localStorage.getItem('LoggedInUser')){
        this.router.navigate(['dashboard']) //if user is loggedin, give them access to code-editor
      } else {
        this.router.navigate(['login']) //route to login page if user isnt loggedin
      }
    });
  }
}
