import React, { useEffect } from "react";

 function App(props) {

    useEffect(()=> {console.log("hola")});
    const clicked = ( )=> {
        console.log("hola");
    }
    return (
        <div>
        <p>Hellooo</p> 
        {/* <button title="clickme" onClick={clicked}/> */}
        </div>
    )
}
export default App