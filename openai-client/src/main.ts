import {createApp} from 'vue'
import './style.scss'
import App from './App.vue'
import "./index.css";
import gsap from "gsap";
import TextPlugin from "gsap/TextPlugin";
import './modules/HighlightJsModule';

gsap.registerPlugin(TextPlugin)

const app = createApp(App)
app.mount('#app')
