using System;
using UnityEngine;

[Serializable]
public class TranscriptionResponse
{
    public string id;
    public string request_id;
    public int version;
    public string status;
    public string created_at;
    public string completed_at;
    public CustomMetadata custom_metadata;
    public int error_code;
    public string kind;
    public APIFile file;
    public RequestParams request_params;
    public Result result;
}

[Serializable]
public class CustomMetadata {}

[Serializable]
public class APIFile
{
    public string id;
    public string filename;
    public string source;
    public float audio_duration;
    public int number_of_channels;
}

[Serializable]
public class RequestParams
{
    public string context_prompt;
    public bool custom_vocabulary;
    public CustomVocabularyConfig custom_vocabulary_config;
    public bool detect_language;
    public bool enable_code_switching;
    public CodeSwitchingConfig code_switching_config;
    public string language;
    public string callback_url;
    public bool callback;
    public CallbackConfig callback_config;
    public bool subtitles;
    public SubtitlesConfig subtitles_config;
    public bool diarization;
    public DiarizationConfig diarization_config;
    public bool translation;
    public TranslationConfig translation_config;
    public bool summarization;
    public SummarizationConfig summarization_config;
    public bool moderation;
    public bool named_entity_recognition;
    public bool chapterization;
    public bool name_consistency;
    public bool custom_spelling;
    public CustomSpellingConfig custom_spelling_config;
    public bool structured_data_extraction;
    public StructuredDataExtractionConfig structured_data_extraction_config;
    public bool sentiment_analysis;
    public bool audio_to_llm;
    public AudioToLLMConfig audio_to_llm_config;
    public bool sentences;
    public bool display_mode;
    public bool punctuation_enhanced;
    public LanguageConfig language_config;
    public string audio_url;
}

[Serializable]
public class CustomVocabularyConfig
{
    public VocabularyWrapper vocabulary;
    public float default_intensity;
}

[Serializable]
public class VocabularyWrapper
{
    public string[] vocabulary; // Unity can't deserialize mixed-type arrays; simplify if needed
    public float default_intensity;
}

[Serializable]
public class CodeSwitchingConfig
{
    public string[] languages;
}

[Serializable]
public class CallbackConfig
{
    public string url;
    public string method;
}

[Serializable]
public class SubtitlesConfig
{
    public string[] formats;
    public float minimum_duration;
    public float maximum_duration;
    public int maximum_characters_per_row;
    public int maximum_rows_per_caption;
    public string style;
}

[Serializable]
public class DiarizationConfig
{
    public int number_of_speakers;
    public int min_speakers;
    public int max_speakers;
    public bool enhanced;
}

[Serializable]
public class TranslationConfig
{
    public string[] target_languages;
    public string model;
    public bool match_original_utterances;
}

[Serializable]
public class SummarizationConfig
{
    public string type;
}

[Serializable]
public class CustomSpellingConfig
{
    public SpellingDictionary spelling_dictionary;
}

[Serializable]
public class SpellingDictionary
{
    public string[] Gettleman;
    public string[] SQL;
}

[Serializable]
public class StructuredDataExtractionConfig
{
    public string[] classes;
}

[Serializable]
public class AudioToLLMConfig
{
    public string[] prompts;
}

[Serializable]
public class LanguageConfig
{
    public string[] languages;
    public bool code_switching;
}

[Serializable]
public class Result
{
    public Metadata metadata;
    public Transcription transcription;
    public Translation translation;
    public Summarization summarization;
    public Moderation moderation;
    public NamedEntityRecognition named_entity_recognition;
    public NameConsistency name_consistency;
    public CustomSpelling custom_spelling;
    public SpeakerReidentification speaker_reidentification;
    public StructuredDataExtraction structured_data_extraction;
    public SentimentAnalysis sentiment_analysis;
    public AudioToLLM audio_to_llm;
    public Sentences sentences;
    public DisplayMode display_mode;
    public Chapters chapters;
}

[Serializable]
public class Metadata
{
    public float audio_duration;
    public int number_of_distinct_channels;
    public float billing_time;
    public float transcription_time;
}

[Serializable]
public class Transcription
{
    public string full_transcript;
    public string[] languages;
    public Sentence[] sentences;
    public Subtitle[] subtitles;
    public Utterance[] utterances;
}

[Serializable]
public class Sentence
{
    public bool success;
    public bool is_empty;
    public float exec_time;
    public Error error;
    public string[] results;
}

[Serializable]
public class Subtitle
{
    public string format;
    public string subtitles;
}

[Serializable]
public class Utterance
{
    public string language;
    public float start;
    public float end;
    public float confidence;
    public int channel;
    public int speaker;
    public Word[] words;
    public string text;
}

[Serializable]
public class Word
{
    public string word;
    public float start;
    public float end;
    public float confidence;
}

[Serializable]
public class Error
{
    public int status_code;
    public string exception;
    public string message;
}

// The rest follow the same pattern
[Serializable] public class Translation : TranscriptionResult {}
[Serializable] public class Summarization : ErrorResult {}
[Serializable] public class Moderation : ErrorResult {}
[Serializable] public class NamedEntityRecognition : ErrorResult { public string entity; }
[Serializable] public class NameConsistency : ErrorResult {}
[Serializable] public class CustomSpelling : ErrorResult {}
[Serializable] public class SpeakerReidentification : ErrorResult {}
[Serializable] public class StructuredDataExtraction : ErrorResult {}
[Serializable] public class SentimentAnalysis : ErrorResult {}
[Serializable] public class Sentences : ErrorResult {}
[Serializable] public class DisplayMode : ErrorResult {}
[Serializable] public class Chapters : ErrorResult {}

[Serializable]
public class AudioToLLM
{
    public bool success;
    public bool is_empty;
    public float exec_time;
    public Error error;
    public AudioToLLMResult[] results;
}

[Serializable]
public class AudioToLLMResult
{
    public bool success;
    public bool is_empty;
    public float exec_time;
    public Error error;
    public PromptResponse results;
}

[Serializable]
public class PromptResponse
{
    public string prompt;
    public string response;
}

[Serializable]
public class TranscriptionResult
{
    public bool success;
    public bool is_empty;
    public float exec_time;
    public Error error;
    public TranslationResult[] results;
}

[Serializable]
public class TranslationResult
{
    public Error error;
    public string full_transcript;
    public string[] languages;
    public Sentence[] sentences;
    public Subtitle[] subtitles;
    public Utterance[] utterances;
}

[Serializable]
public class ErrorResult
{
    public bool success;
    public bool is_empty;
    public float exec_time;
    public Error error;
    public string results;
}
