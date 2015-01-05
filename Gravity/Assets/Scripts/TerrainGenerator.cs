using UnityEngine;
using System.Collections;



public class TerrainGenerator : MonoBehaviour {
	public GameObject block;
	public int size = 10;
	private int noiseFactor = 20;
	private int seed; 
	private byte [,,] chunks;

	// Use this for initialization
	void Start () {
		seed =  Random.Range(0, 1000);
		chunks = new byte[size, size, size];
		fillChunks ();
		convertMatrixToChunks ();
	}

	void fillChunks (){
		for (int i = 0; i<size; i++) {
			for (int k = 0; k<size; k++) {
				float height = Mathf.PerlinNoise((float)(i+seed)/noiseFactor, (float)(k+seed)/noiseFactor)*size;
				for (int j = 0; j<height; j++){
					chunks[i,j,k] = 1;
				}
			}
		}
		Debug.Log ("done");
	}

	void convertMatrixToChunks (){
		for (int x = 0; x< chunks.GetLength(0); x++) {
			for (int z = 0; z< chunks.GetLength(1); z++){
				for(int y = 0; y< chunks.GetLength(2); y++){
					renderBlock(x, y, z);
				}
			}
		}
	}

	void renderBlock(int x, int y, int z){
		//Only render a block if, out of its five adjacent blocks (do not count above)
		//there is a block AND an empty space

		if (chunks [x, y, z] == null) {
			chunks[x,y,z]=0;		
		} else if (chunks [x, y, z] == 1) {
			if(x == 0 || y == 0 || z == 0 || x == (size-1) || z == (size-1)){
				Instantiate(block, new Vector3(x, y, z), Quaternion.identity);
			}else{
				int surroundingBlocks = 0;
				if(chunks[x+1, y, z]==1){
					surroundingBlocks++;
				}
				if(chunks[x-1, y, z]==1){
					surroundingBlocks++;
				}
				if(chunks[x, y, z+1]==1){
					surroundingBlocks++;
				}
				if(chunks[x, y, z-1]==1){
					surroundingBlocks++;
				}
				if(chunks[x, y-1, z]==1){
					surroundingBlocks++;
				}
				if(chunks[x, y+1, z]==1){
					surroundingBlocks++;
				}
				if(surroundingBlocks>0&&surroundingBlocks<6){
					Instantiate(block, new Vector3(x, y, z), Quaternion.identity);
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	byte[,] generate64x64PerlinNoise (){
		return new byte[0, 0];
	}

	byte smooth2DNoise (byte x, byte y){
		return 0;
	}
	byte Noise2D (byte x, byte y){
		Random.seed = 100*x+y;
		return (byte) Random.Range (0, 255);
	}
}
